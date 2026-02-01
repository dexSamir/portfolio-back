using Microsoft.AspNetCore.Http;
namespace Portfolio.Application.Dtos.Project;

public class ProjectUpdateDto
{
    public string? Title { get;set; } = null!;
    public string? Description { get;set; } = null!;
    public IFormFile? ImageUrl { get; set; }
    public string? ExistingImageUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? GithubUrl { get; set; }
    
    public List<Guid>? TechnologyIds { get; set; }
}