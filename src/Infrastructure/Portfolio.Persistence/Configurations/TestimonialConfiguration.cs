using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Configurations;
public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Company)
            .HasMaxLength(100);

        builder.Property(x => x.Position)
            .HasMaxLength(100);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => new { x.Status, x.CreatedTime });
        builder.HasIndex(x => x.Rating);
        builder.HasIndex(x => x.CreatedTime);
    }
}