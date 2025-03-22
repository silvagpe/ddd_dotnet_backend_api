using System.Reflection;

namespace DeveloperStore.Api.DependencyInjections;

public static class MediatRInjections
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));                
        return services;
    }
}