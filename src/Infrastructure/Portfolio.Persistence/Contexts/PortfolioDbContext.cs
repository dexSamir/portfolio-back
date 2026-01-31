using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Contexts;

public class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
    : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<Project> Projects { get; set; }
     
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(PortfolioDbContext).Assembly); 
        base.OnModelCreating(builder);
    }}