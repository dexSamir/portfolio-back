using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Infrastructure.ExternalServices;

namespace Portfolio.Infrastructure;

public static class ExternalServiceRegistration
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>(); 
        return services;
    }
}