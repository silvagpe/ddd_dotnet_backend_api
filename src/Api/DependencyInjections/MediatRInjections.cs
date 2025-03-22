using System.Reflection;
using DeveloperStore.Application;

namespace DeveloperStore.Api.DependencyInjections;

public static class MediatRInjections
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));                
        return services;
    }
}