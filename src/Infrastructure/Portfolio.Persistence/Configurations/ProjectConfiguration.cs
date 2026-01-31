using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Configurations;

public class ProjectConfiguration 
    : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.LiveUrl)
            .HasMaxLength(500);

        builder.Property(x => x.GithubUrl)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedTime)
            .IsRequired();

        builder.Property(x => x.UpdatedTime)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasMany(x => x.ProjectTechnologies)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId);

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.IsDeleted);
        builder.HasIndex(x => x.CreatedTime);
        builder.HasIndex(x => new { x.IsDeleted, x.CreatedTime });
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}