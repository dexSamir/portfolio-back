using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Infrastructure.ExternalServices;

namespace Portfolio.Infrastructure.ServiceRegistrations;

public static class ExternalServiceRegistration
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>(); 
        return services;
    }
    
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "Portfolio_";
        });

        return services;
    }
    
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}