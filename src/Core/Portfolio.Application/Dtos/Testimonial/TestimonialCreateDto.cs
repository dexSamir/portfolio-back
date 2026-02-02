using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Application.Dtos.Testimonial;

public class TestimonialCreateDto
{
    [Required]
    public string FullName { get; set; } = null!;

    public IFormFile? ProfileImage { get; set; }

    public string? Company { get; set; }
    public string? Position { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = null!;

    [Range(1, 5)]
    public int Rating { get; set; }
}