using Portfolio.Application.Dtos.Technology;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Abstraction.Services;

public interface ITechnologyService
{
    Task<IEnumerable<TechnologyGetDto>> GetAllAsync();
    Task<TechnologyGetDto> CreateAsync(TechnologyCreateDto dto);
    Task<IEnumerable<TechnologyGetDto>> CreateBulkAsync(IEnumerable<TechnologyCreateDto> dtos); 
    Task<TechnologyGetDto> GetByIdAsync(Guid id);
    Task<TechnologyGetDto> UpdateAsync(Guid id,TechnologyUpdateDto dto); 
    Task<bool> DeleteAsync(Guid[] publicIds, EDeleteType dType); 
}