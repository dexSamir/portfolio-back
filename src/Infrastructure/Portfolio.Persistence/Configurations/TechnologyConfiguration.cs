using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Configurations;

public class TechnologyConfiguration : IEntityTypeConfiguration<Technology>
{
    public void Configure(EntityTypeBuilder<Technology> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.CreatedTime)
            .IsRequired();

        builder.Property(x => x.UpdatedTime)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasMany(x => x.ProjectTechnologies)
            .WithOne(x => x.Technology)
            .HasForeignKey(x => x.TechnologyId);

        builder.HasIndex(x => x.IsDeleted);
        builder.HasIndex(x => x.Name);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}