using Portfolio.Application.Dtos.Testimonial;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Abstraction.Services;

public interface ITestimonialService
{
    Task<IEnumerable<TestimonialGetDto>> GetApprovedAsync();
    Task<IEnumerable<TestimonialGetDto>> GetAllAsync();
    Task<IEnumerable<TestimonialGetDto>> GetByStatusAsync(ETestimonialStatus status);
    Task<TestimonialGetDto> CreateAsync(TestimonialCreateDto dto);
    Task<bool> ChangeStatusAsync(Guid id, ETestimonialStatus status);
    Task<bool> DeleteAsync(Guid id);
}