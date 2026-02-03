using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Infrastructure.ExternalServices;

namespace Portfolio.Infrastructure.ServiceRegistrations;

public static class ExternalServiceRegistration
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IFileService, FileService>(); 
        return services;
    }
    
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
    
    
}