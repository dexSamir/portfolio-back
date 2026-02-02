using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Contexts;

public class PortfolioDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
        : base(options)
    {
    }

    public DbSet<Technology> Technologies { get; set; }
    public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
     
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(PortfolioDbContext).Assembly); 
        base.OnModelCreating(builder);
    }
}
