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

public class ProjectService(IProjectRepository repo, IMapper mapper, ICacheService cache) : IProjectService
{
    public async Task<IEnumerable<ProjectGetDto>> GetAllAsync()
    {
        var projects = await cache.GetOrSetAsync(
            CacheKeys.Project,
            () =>  repo.GetAllAsync(),
            TimeSpan.FromMinutes(5)); 
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
        data.CreatedTime = DateTime.UtcNow;

        await repo.AddAsync(data);
        await repo.SaveAsync();
        await repo.GetByIdAsync(data.Id, true, "ProjectTechnologies", "ProjectTechnologies.Technology");
        return mapper.Map<ProjectGetDto>(data); 
    }

    public async Task<IEnumerable<ProjectGetDto>> CreateBulkAsync(IEnumerable<ProjectGetDto> dtos)
    {
        var dtoList = dtos.ToList();
        var data = mapper.Map<IList<Project>>(dtoList);
        
        foreach (var project in data)
            project.CreatedTime = DateTime.UtcNow;

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

        mapper.Map(dto, existing);
        existing.UpdatedTime = DateTime.UtcNow;
        
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
            .Where(pt => removeTechIds.Any(id => id == pt.TechnologyId))
            .ToList();


        foreach (var pt in toRemove)
            existing.ProjectTechnologies.Remove(pt);

        await repo.UpdateAsync(existing);
        await repo.SaveAsync();

        return mapper.Map<ProjectGetDto>(existing);
    }
    
    public Task<bool> RestoreAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, EDeleteType deleteType)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectGetDto> AddTechnologiesAsync(Guid projectId, IEnumerable<Guid> techIds)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectGetDto> RemoveTechnologiesAsync(Guid projectId, IEnumerable<Guid> techIds)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProjectGetDto>> GetByTechnologyAsync(Guid[] technologyIds)
    {
        throw new NotImplementedException();
    }
}