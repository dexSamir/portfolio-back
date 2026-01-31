using AutoMapper;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Technology;
using Portfolio.Common.Constants;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Services;

public class TechnologyService(ITechnologyRepository repo, IMapper mapper, ICacheService cache) : ITechnologyService
{
    public async Task<IEnumerable<TechnologyGetDto>> GetAllAsync()
    {
        var technologies =  await cache.GetOrSetAsync(
            CacheKeys.Technology,
            () => repo.GetAllAsync(),
            TimeSpan.FromMinutes(2)
        );

        return  mapper.Map<IEnumerable<TechnologyGetDto>>(technologies);
    }

    public Task<TechnologyGetDto> CreateAsync(TechnologyCreateDto dto)
    {
        return _technologyServiceImplementation.CreateAsync(dto);
    }

    public Task<TechnologyGetDto> GetByIdAsync(Guid id)
    {
        return _technologyServiceImplementation.GetByIdAsync(id);
    }

    public Task<TechnologyGetDto> UpdateAsync(Guid id, TechnologyUpdateDto dto)
    {
        return _technologyServiceImplementation.UpdateAsync(id, dto);
    }

    public Task<bool> DeleteAsync(Guid[] publicIds, EDeleteType dType)
    {
        return _technologyServiceImplementation.DeleteAsync(publicIds, dType);
    }
}