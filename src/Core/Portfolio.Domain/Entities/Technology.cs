using Portfolio.Domain.Entities.Base;

namespace Portfolio.Domain.Entities;

public class Technology : BaseEntity
{
    public string Name { get; set; } = null!; 
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}