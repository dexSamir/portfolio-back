using AutoMapper;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Project;
using Portfolio.Application.Exceptions.Common;
using Portfolio.Common.Constants;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Services;

public class ProjectService(
    IProjectRepository repo,
    IMapper mapper,
    ICacheService cache,
    ICloudinaryService cloudinary
) : IProjectService
{
    public async Task<IEnumerable<ProjectGetDto>> GetAllAsync()
    {
        var projects = await cache.GetOrSetAsync(
            CacheKeys.Project,
            () => repo.GetAllAsync(
                asNoTrack: true,
                includes: new[]
                {
                    "ProjectTechnologies",
                    "ProjectTechnologies.Technology"
                }
            ),
            TimeSpan.FromMinutes(5)
        );

        return mapper.Map<IEnumerable<ProjectGetDto>>(projects);
    }

    public async Task<ProjectGetDto> GetByIdAsync(Guid id)
    {
        var data = await repo.GetFirstAsync(
            x => x.Id == id,
            asNoTrack: true,
            "ProjectTechnologies.Technology"
        ) ?? throw new NotFoundException<Project>();

        return mapper.Map<ProjectGetDto>(data);
    }

    public async Task<ProjectGetDto> CreateAsync(ProjectCreateDto dto)
    {
        var entity = mapper.Map<Project>(dto);
        entity.CreatedTime = DateTime.UtcNow;

        if (dto.ImageUrl != null)
        {
            entity.ImageUrl = await cloudinary.UploadImageAsync(
                dto.ImageUrl,
                "projects"
            );
        }

        await repo.AddAsync(entity);
        await repo.SaveAsync();

        await cache.RemoveAsync(CacheKeys.Project);

        return mapper.Map<ProjectGetDto>(entity);
    }

    public async Task<IEnumerable<ProjectGetDto>> CreateBulkAsync(IEnumerable<ProjectCreateDto> dtos)
    {
        var entities = new List<Project>();

        foreach (var dto in dtos)
        {
            var project = mapper.Map<Project>(dto);
            project.CreatedTime = DateTime.UtcNow;

            if (dto.ImageUrl != null)
            {
                project.ImageUrl = await cloudinary.UploadImageAsync(
                    dto.ImageUrl,
                    "projects"
                );
            }

            entities.Add(project);
        }

        await repo.AddRangeAsync(entities);
        await repo.SaveAsync();
        await cache.RemoveAsync(CacheKeys.Project);

        return mapper.Map<IEnumerable<ProjectGetDto>>(entities);
    }

    public async Task<ProjectGetDto> UpdateAsync(Guid id, ProjectUpdateDto dto)
    {
        var entity = await repo.GetFirstAsync(
            x => x.Id == id,
            asNoTrack: false,
            "ProjectTechnologies",
            "ProjectTechnologies.Technology"
        ) ?? throw new NotFoundException<Project>();

        if (dto.ImageUrl != null)
        {
            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                // 👇 SƏNİN DEDİYİN SƏTR – BURADA TAM DOĞRUDUR
                await cloudinary.DeleteImageAsync(entity.ImageUrl);
            }

            entity.ImageUrl = await cloudinary.UploadImageAsync(
                dto.ImageUrl,
                "projects"
            );
        }

        entity.UpdatedTime = DateTime.UtcNow;
        mapper.Map(dto, entity);

        await repo.UpdateAsync(entity);
        await repo.SaveAsync();
        await cache.RemoveAsync(CacheKeys.Project);

        return mapper.Map<ProjectGetDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid[] ids, EDeleteType dType)
    {
        if (ids == null || ids.Length == 0)
            throw new ArgumentException("Heç bir id daxil edilməyib!");

        var projects = await repo.GetWhereAsync(x => ids.Contains(x.Id), false);

        if (dType == EDeleteType.Hard)
        {
            foreach (var project in projects)
            {
                if (!string.IsNullOrEmpty(project.ImageUrl))
                    await cloudinary.DeleteImageAsync(project.ImageUrl);
            }

            await repo.HardDeleteRangeAsync(ids);
        }
        else if (dType == EDeleteType.Soft)
        {
            await repo.SoftDeleteRangeAsync(ids);
        }
        else if (dType == EDeleteType.Reverse)
        {
            await repo.ReverseDeleteRangeAsync(ids);
        }

        var success = await repo.SaveAsync() > 0;

        if (success)
            await cache.RemoveAsync(CacheKeys.Project);

        return success;
    }

    public async Task<bool> RestoreAsync(Guid[] ids)
        => await DeleteAsync(ids, EDeleteType.Reverse);
    
    
    public async Task<IEnumerable<ProjectGetDto>> GetByTechnologyAsync(Guid[] technologyIds)
    {
        var techIds = technologyIds.ToList();

        var projects = await repo.GetWhereAsync(
            p => p.ProjectTechnologies.Any(pt => techIds.Contains((Guid)pt.TechnologyId!)),
            asNoTrack: true,
            "ProjectTechnologies",
            "ProjectTechnologies.Technology"
        );
        return mapper.Map<IEnumerable<ProjectGetDto>>(projects);
    }
    
    public async Task<ProjectGetDto> AddTechnologiesAsync(Guid projectId, IEnumerable<Guid>? techIds)
    {
        if (techIds == null || !techIds.Any())
            return await GetByIdAsync(projectId);

        var project = await repo.GetFirstAsync(
            x => x.Id == projectId,
            asNoTrack: false,
            "ProjectTechnologies"
        ) ?? throw new NotFoundException<Project>();

        var currentTechIds = (project.ProjectTechnologies ?? new List<ProjectTechnology>())
            .Select(pt => pt.TechnologyId)
            .ToHashSet();

        foreach (var techId in techIds)
            if (!currentTechIds.Contains(techId))
            {
                project.ProjectTechnologies ??= new List<ProjectTechnology>();
                project.ProjectTechnologies.Add(new ProjectTechnology
                {
                    ProjectId = project.Id,
                    TechnologyId = techId
                });
            }

        await repo.UpdateAsync(project);
        await repo.SaveAsync();

        return mapper.Map<ProjectGetDto>(project);
    }
    
    public async Task<ProjectGetDto> RemoveTechnologiesAsync(Guid projectId, IEnumerable<Guid> techIds)
    {
        var project = await repo.GetFirstAsync(
            x => x.Id == projectId,
            asNoTrack: false,
            "ProjectTechnologies"
        ) ?? throw new NotFoundException<Project>();

        var idsToRemove = techIds?.ToList() ?? new List<Guid>();

        var toRemove = project.ProjectTechnologies
            .Where(pt => idsToRemove.Contains((Guid)pt.TechnologyId!))
            .ToList();

        foreach (var pt in toRemove)
            project.ProjectTechnologies.Remove(pt);

        await repo.UpdateAsync(project);
        await repo.SaveAsync();

        return mapper.Map<ProjectGetDto>(project);
    }
}
