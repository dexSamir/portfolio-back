using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Configurations;

public class ProjectTechnologyConfiguration 
    : IEntityTypeConfiguration<ProjectTechnology>
{
    public void Configure(EntityTypeBuilder<ProjectTechnology> builder)
    {
        builder.HasKey(x => new { x.ProjectId, x.TechnologyId });

        builder.HasOne(x => x.Project)
            .WithMany(x => x.ProjectTechnologies)
            .HasForeignKey(x => x.ProjectId);

        builder.HasOne(x => x.Technology)
            .WithMany(x => x.ProjectTechnologies)
            .HasForeignKey(x => x.TechnologyId);

        builder.HasIndex(x => x.ProjectId);
        builder.HasIndex(x => x.TechnologyId);
    }
}