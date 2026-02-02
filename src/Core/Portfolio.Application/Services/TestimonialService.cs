using AutoMapper;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Testimonial;
using Portfolio.Application.Exceptions.Common;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Services;

public class TestimonialService(ITestimonialRepository repo, IMapper mapper, IFileService fileService)
    : ITestimonialService
{
    public async Task<TestimonialGetDto> CreateAsync(TestimonialCreateDto dto)
    {
        var entity = mapper.Map<Testimonial>(dto);
        entity.Status = ETestimonialStatus.Pending;
        entity.CreatedTime = DateTime.UtcNow;

        if (dto.ProfileImage != null)
            entity.ProfileImageUrl = await fileService.ProcessImageAsync(dto.ProfileImage, "testimonials", "image/", 5);

        await repo.AddAsync(entity);
        await repo.SaveAsync();

        return mapper.Map<TestimonialGetDto>(entity);
    }

    public async Task<IEnumerable<TestimonialGetDto>> GetApprovedAsync()
    {
        var data = await repo.GetWhereAsync(t => t.Status == ETestimonialStatus.Approved, asNoTrack: true);
        return mapper.Map<IEnumerable<TestimonialGetDto>>(data);
    }

    public async Task<IEnumerable<TestimonialGetDto>> GetAllAsync()
    {
        var data = await repo.GetAllAsync(asNoTrack: true);
        return mapper.Map<IEnumerable<TestimonialGetDto>>(data);
    }

    public async Task<IEnumerable<TestimonialGetDto>> GetByStatusAsync(ETestimonialStatus status)
    {
        var data = await repo.GetWhereAsync(t => t.Status == status, asNoTrack: true);
        return mapper.Map<IEnumerable<TestimonialGetDto>>(data);
    }

    public async Task<bool> ChangeStatusAsync(Guid id, ETestimonialStatus status)
    {
        var entity = await repo.GetByIdAsync(id, false) ?? throw new NotFoundException<Testimonial>();
        entity.Status = status;
        await repo.UpdateAsync(entity);
        return await repo.SaveAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repo.GetByIdAsync(id, false) ?? throw new NotFoundException<Testimonial>();

        if (!string.IsNullOrEmpty(entity.ProfileImageUrl))
            await fileService.DeleteImageIfNotDefault(entity.ProfileImageUrl, "testimonials");

        await repo.HardDeleteAsync(id);
        return await repo.SaveAsync() > 0;
    }
}
