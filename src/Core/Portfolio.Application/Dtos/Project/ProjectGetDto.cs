using Microsoft.AspNetCore.Http;
using Portfolio.Application.Dtos.Technology;

namespace Portfolio.Application.Dtos.Project;

public class ProjectGetDto
{
    public Guid Id { get;set; }
    public string Title { get;set; } = null!;
    public string Description { get;set; } = null!;
    public IFormFile ImageUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? GithubUrl { get; set; }
    public DateTime CreatedTime { get;set; }
    public DateTime UpdatedTime { get;set; }
    public bool IsDeleted { get;init; }
    
    public List<TechnologyNestedGetDto> Technologies { get; set; }
    
}