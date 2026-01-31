using Microsoft.Extensions.DependencyInjection;

namespace Portfolio.Infrastructure.ServiceRegistrations;

public static class CacheServiceRegistration
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "Portfolio_";
        });

        return services;
    }
}