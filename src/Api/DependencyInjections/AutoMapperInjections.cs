using DeveloperStore.Application;

namespace DeveloperStore.Api.DependencyInjections;

public static class AutoMapperInjections
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationAssemblyMarker));
        
        return services;
    }
    
}