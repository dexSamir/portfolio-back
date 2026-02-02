using Portfolio.Domain.Entities.Base;
using Portfolio.Domain.Enums;

namespace Portfolio.Domain.Entities;

public class Testimonial : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }

    public string Message { get; set; } = null!;
    public int Rating { get; set; }

    public ETestimonialStatus Status { get; set; } = ETestimonialStatus.Pending;
}