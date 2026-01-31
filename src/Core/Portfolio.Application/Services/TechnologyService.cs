using AutoMapper;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Technology;
using Portfolio.Application.Exceptions.Common;
using Portfolio.Common.Constants;
using Portfolio.Domain.Entities;
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
    
    public async Task<TechnologyGetDto> GetByIdAsync(Guid id)
    {
        var data = await repo.GetByIdAsync(id, true, "ProjectTechnology") ?? 
                   throw new NotFoundException<Technology>();
        return mapper.Map<TechnologyGetDto>(data); 
    }
    
    public async Task<TechnologyGetDto> CreateAsync(TechnologyCreateDto dto)
    {
        var data = mapper.Map<Technology>(dto);
        data.CreatedTime = DateTime.UtcNow;

        await repo.AddAsync(data);
        await repo.SaveAsync();
        return mapper.Map<TechnologyGetDto>(data); 
    }
    
    public async Task<IEnumerable<TechnologyGetDto>> CreateBulkAsync(IEnumerable<TechnologyCreateDto> dtos)
    {
        var dtoList = dtos.ToList();
        var data = mapper.Map<IList<Technology>>(dtoList);

        for (int i = 0; i < data.Count; i++)
            data[i].CreatedTime = DateTime.UtcNow;

        await repo.AddRangeAsync(data);
        await repo.SaveAsync();
        return mapper.Map<IEnumerable<TechnologyGetDto>>(data);
    }
    
    public async Task<TechnologyGetDto> UpdateAsync(Guid id, TechnologyUpdateDto dto)
    {
        var existing = await repo.GetFirstAsync(x=> x.Id == id, false) ?? throw new NotFoundException<Technology>();

        mapper.Map(dto, existing);
        existing.UpdatedTime = DateTime.UtcNow;

        await repo.UpdateAsync(existing);
        await repo.SaveAsync();

        return mapper.Map<TechnologyGetDto>(existing);
    }
    
    public async Task<bool> DeleteAsync(Guid[] ids, EDeleteType dType)
    {
        if (ids.Length == 0)
            throw new ArgumentException("Hec bir id daxil edilmeyib!");

        var existingIds = (await repo.GetWhereAsync(x=> ids.Contains(x.Id), false))
            .Select(x => x.Id)
            .ToArray();
        
        var missingIds = existingIds.Except(existingIds).ToArray();

        if (missingIds.Any())
            throw new NotFoundException<Technology>($"Products not found with ids: {string.Join(",", missingIds)}");

        
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
                throw new UnsupportedDeleteTypeException($"Delete type '{dType}' is not supported.");
        }

        bool success = await repo.SaveAsync() == existingIds.Length;

        return success;
    }
}