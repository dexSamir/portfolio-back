using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Persistence.Repositories;

namespace Portfolio.Persistence;

public static class RepositoryRegistration
{
    public static IServiceCollection AddPersistentServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>(); 
        services.AddScoped<ITechnologyRepository, TechnologyRepository>();
        return services;
    }
}