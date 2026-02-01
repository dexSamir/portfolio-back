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

public class ProjectService(IProjectRepository repo, IMapper mapper, ICacheService cache, IFileService fileService) : IProjectService
{
    public async Task<IEnumerable<ProjectGetDto>> GetAllAsync()
    {
        var projects = await cache.GetOrSetAsync(
            CacheKeys.Project,
            () => repo.GetAllAsync(
                asNoTrack: true,
                predicate: null,
                orderBy: null,
                "ProjectTechnologies",
                "ProjectTechnologies.Technology"
            ),
            TimeSpan.FromMinutes(5)
        );

        return mapper.Map<IEnumerable<ProjectGetDto>>(projects);
    }


    public async Task<ProjectGetDto> GetByIdAsync(Guid id)
    {
        var data = await repo.GetFirstAsync(
                       x=> x.Id == id,
                       asNoTrack:true,
                       "ProjectTechnologies.Technology")
                   ?? throw new NotFoundException<Project>();
        return mapper.Map<ProjectGetDto>(data); 
    }

    public async Task<ProjectGetDto> CreateAsync(ProjectCreateDto dto)
    {
        var data = mapper.Map<Project>(dto);
        await repo.GetByIdAsync(data.Id, true, "ProjectTechnologies", "ProjectTechnologies.Technology");
        data.CreatedTime = DateTime.UtcNow;

        if (dto?.ImageUrl != null)
            data.ImageUrl = await fileService.ProcessImageAsync(dto.ImageUrl, "projects", "image/", 15); 
                
        await repo.AddAsync(data);
        await repo.SaveAsync();
        return mapper.Map<ProjectGetDto>(data); 
    }

    public async Task<IEnumerable<ProjectGetDto>> CreateBulkAsync(IEnumerable<ProjectCreateDto> dtos)
    {
        var dtoList = dtos.ToList();
        var data = mapper.Map<IList<Project>>(dtoList);

        for (int i = 0; i < dtoList.Count; i++)
        {
            data[i].CreatedTime = DateTime.UtcNow;
            if (dtos.ElementAt(i).ImageUrl != null)
            {
                await repo.GetByIdAsync(data[i].Id, true, "ProjectTechnologies", "ProjectTechnologies.Technology");
                data[i].ImageUrl = await fileService.ProcessImageAsync(dtos.ElementAt(i).ImageUrl, "projects", "image/", 15);
            }
        }

        await repo.AddRangeAsync(data);
        await repo.SaveAsync();
        return mapper.Map<IEnumerable<ProjectGetDto>>(data);
    }

    public async Task<ProjectGetDto> UpdateAsync(Guid id, ProjectUpdateDto dto)
    {
        var existing = await repo.GetFirstAsync(
            x => x.Id == id, 
            false, 
            "ProjectTechnologies", 
            "ProjectTechnologies.Technology"
        ) ?? throw new NotFoundException<Project>();

        
        if(dto.ImageUrl != null)
            existing.ImageUrl = await fileService.ProcessImageAsync(dto.ImageUrl, "projects", "image/", 15, existing.ImageUrl);
        
        existing.UpdatedTime = DateTime.UtcNow;
        dto.ExistingImageUrl = existing.ImageUrl;
        mapper.Map(dto, existing);
        
        var dtoTechIds = dto.TechnologyIds ?? new List<Guid>();
        var currentTechIds = existing.ProjectTechnologies.Select(pt => pt.TechnologyId).ToList();

        var newTechIds = dtoTechIds.Where(guid => !currentTechIds.Contains(guid)).ToList();
        var removeTechIds = currentTechIds.Where(guid => !dtoTechIds.Contains((Guid)guid!)).ToList();

        foreach (var techId in newTechIds)
        {
            existing.ProjectTechnologies.Add(new ProjectTechnology
            {
                ProjectId = existing.Id,
                TechnologyId = techId
            });
        }


        var toRemove = existing.ProjectTechnologies
            .Where(pt => removeTechIds.Any(guid => guid == pt.TechnologyId))
            .ToList();


        foreach (var pt in toRemove)
            existing.ProjectTechnologies.Remove(pt);

        await repo.UpdateAsync(existing);
        await repo.SaveAsync();

        return mapper.Map<ProjectGetDto>(existing);
    }
    
    public async Task<bool> DeleteAsync(Guid[] ids, EDeleteType dType)
    {
        if (ids == null || ids.Length == 0)
            throw new ArgumentException("Hec bir id daxil edilmeyib!");

        var existingIds = (await repo.GetWhereAsync(x => ids.Contains(x.Id), false))
            .Select(x => x.Id)
            .ToArray();

        if(dType == EDeleteType.Hard)
            foreach (var id in ids)
            {
                var data = await repo.GetByIdAsync(id, false) ?? throw new NotFoundException<Project>();
                if(!string.IsNullOrEmpty(data.ImageUrl))
                    await fileService.DeleteImageIfNotDefault(data.ImageUrl, "projects");
            }

        
        var missingIds = ids.Except(existingIds).ToArray();
        if (missingIds.Any())
            throw new NotFoundException<Technology>(
                $"Products not found with ids: {string.Join(",", missingIds)}");

        switch (dType)
        {
            case EDeleteType.Soft:
                await repo.SoftDeleteRangeAsync(existingIds);
                break;

            case EDeleteType.Reverse:
                await repo.ReverseDeleteRangeAsync(existingIds);
                break;

            case EDeleteType.Hard:
                await repo.HardDeleteRangeAsync(existingIds);
                break;

            default:
                throw new UnsupportedDeleteTypeException(
                    $"Delete type '{dType}' is not supported.");
        }

        var success = await repo.SaveAsync() == ids.Length ? true : false;
        
        if(success)
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