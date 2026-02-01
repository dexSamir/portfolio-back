using Portfolio.Application.Dtos.Project;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Abstraction.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectGetDto>> GetAllAsync();
    Task<ProjectGetDto> GetByIdAsync(Guid id);
    Task<ProjectGetDto> CreateAsync(ProjectCreateDto dto);
    Task<IEnumerable<ProjectGetDto>> CreateBulkAsync(IEnumerable<ProjectGetDto> dtos);
    Task<ProjectGetDto> UpdateAsync(Guid id, ProjectUpdateDto dto); 
    Task<bool> DeleteAsync(Guid id, EDeleteType deleteType);
    Task<ProjectGetDto> AddTechnologiesAsync(Guid projectId, IEnumerable<Guid> techIds);
    Task<ProjectGetDto> RemoveTechnologiesAsync(Guid projectId, IEnumerable<Guid> techIds);
    Task<bool> RestoreAsync(Guid id);
    Task<IEnumerable<ProjectGetDto>> GetByTechnologyAsync(Guid[] technologyIds); 
}