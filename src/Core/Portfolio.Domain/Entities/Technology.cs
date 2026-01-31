using Portfolio.Domain.Entities.Base;

namespace Portfolio.Domain.Entities;

public class Technology : BaseEntity
{
    public string Name { get; set; } = null!; 
    
    public ICollection<ProjectTechnology> ProjectTechnologies { get; set; }

}