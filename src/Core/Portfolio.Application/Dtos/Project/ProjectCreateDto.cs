using Microsoft.AspNetCore.Http;
using Portfolio.Application.Dtos.Technology;

namespace Portfolio.Application.Dtos.Project;

public class ProjectCreateDto
{
    public string Title { get;set; } = null!;
    public string Description { get;set; } = null!;
    public IFormFile? ImageUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? GithubUrl { get; set; }
    
    public List<Guid> TechnologyIds { get; set; }
}