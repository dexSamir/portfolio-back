using Portfolio.Domain.Entities.Base;

namespace Portfolio.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get;set; } = null!;
    public string Description { get;set; } = null!;
    public string? ImageUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? GithubUrl { get; set; }

    public ICollection<Technology> Technologies { get; set; } = null!;
}