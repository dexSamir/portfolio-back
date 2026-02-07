using AutoMapper;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Testimonial;
using Portfolio.Application.Exceptions.Common;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Services;

public class TestimonialService(
    ITestimonialRepository repo,
    IMapper mapper,
    ICloudinaryService cloudinary
) : ITestimonialService
{
    public async Task<TestimonialGetDto> CreateAsync(TestimonialCreateDto dto)
    {
        var entity = mapper.Map<Testimonial>(dto);
        entity.Status = ETestimonialStatus.Pending;
        entity.CreatedTime = DateTime.UtcNow;

        if (dto.ProfileImage != null)
        {
            entity.ProfileImageUrl = await cloudinary.UploadImageAsync(
                dto.ProfileImage,
                "testimonials"
            );
        }

        await repo.AddAsync(entity);
        await repo.SaveAsync();

        return mapper.Map<TestimonialGetDto>(entity);
    }

    public async Task<IEnumerable<TestimonialGetDto>> GetApprovedAsync()
    {
        var data = await repo.GetWhereAsync(
            x => x.Status == ETestimonialStatus.Approved,
            asNoTrack: true
        );

        return mapper.Map<IEnumerable<TestimonialGetDto>>(data);
    }

    public async Task<IEnumerable<TestimonialGetDto>> GetAllAsync()
    {
        var data = await repo.GetAllAsync(asNoTrack: true);
        return mapper.Map<IEnumerable<TestimonialGetDto>>(data);
    }

    public async Task<IEnumerable<TestimonialGetDto>> GetByStatusAsync(ETestimonialStatus status)
    {
        var data = await repo.GetWhereAsync(
            x => x.Status == status,
            asNoTrack: true
        );

        return mapper.Map<IEnumerable<TestimonialGetDto>>(data);
    }

    public async Task<bool> ChangeStatusAsync(Guid id, ETestimonialStatus status)
    {
        var entity = await repo.GetByIdAsync(id, false)
            ?? throw new NotFoundException<Testimonial>();

        entity.Status = status;
        await repo.UpdateAsync(entity);

        return await repo.SaveAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repo.GetByIdAsync(id, false)
            ?? throw new NotFoundException<Testimonial>();

        if (!string.IsNullOrEmpty(entity.ProfileImageUrl))
        {
            await cloudinary.DeleteImageAsync(entity.ProfileImageUrl);
        }

        await repo.HardDeleteAsync(id);
        return await repo.SaveAsync() > 0;
    }
}
