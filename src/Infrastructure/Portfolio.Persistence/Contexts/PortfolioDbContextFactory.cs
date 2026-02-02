using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Portfolio.Persistence.Contexts;

public class PortfolioDbContextFactory : IDesignTimeDbContextFactory<PortfolioDbContext>
{
    public PortfolioDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PortfolioDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=dpg-d60c5iaqcgvc73ace830-a.frankfurt-postgres.render.com;Database=portfoliodb_sp2a;Username=portfoliodb_sp2a_user;Password=URVjH7hBVhD224COAdoE9PzH0oIfy0xv;SSL Mode=Require"
        );

        return new PortfolioDbContext(optionsBuilder.Options);
    }
}