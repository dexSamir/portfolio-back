using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstraction.Repositories;

namespace Portfolio.Persistence;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IGenericRepository, IGenericRepository>(); 

    }
}