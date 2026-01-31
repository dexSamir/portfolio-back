using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Services;

namespace Portfolio.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITechnologyService, TechnologyService>();
        return services;
    }
}