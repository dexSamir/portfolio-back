using Portfolio.Application.Dtos.Project;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Abstraction.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectGetDto>> GetAllAsync();
    Task<ProjectGetDto> GetByIdAsync(Guid id);
    Task<ProjectGetDto> CreateAsync(ProjectCreateDto dto);
    Task<IEnumerable<ProjectGetDto>> CreateBulkAsync(IEnumerable<ProjectCreateDto> dtos);
    Task<ProjectGetDto> UpdateAsync(Guid id, ProjectUpdateDto dto); 
    Task<bool> DeleteAsync(Guid[] ids, EDeleteType dType);
    Task<ProjectGetDto> AddTechnologiesAsync(Guid projectId, IEnumerable<Guid> techIds);
    Task<ProjectGetDto> RemoveTechnologiesAsync(Guid projectId, IEnumerable<Guid> techIds);
    Task<bool> RestoreAsync(Guid[] ids);
    Task<IEnumerable<ProjectGetDto>> GetByTechnologyAsync(Guid[] technologyIds); 
}