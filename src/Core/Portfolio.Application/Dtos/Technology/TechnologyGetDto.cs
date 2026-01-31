namespace Portfolio.Application.Dtos.Technology;

public class TechnologyGetDto
{
    public string Name { get; set; } = null!; 
    public Guid Id { get; init; } 
    public bool IsDeleted { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
}