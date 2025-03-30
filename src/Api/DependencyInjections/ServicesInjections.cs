using DeveloperStore.Domain.Services;
using DeveloperStore.Infrastructure.Services;

namespace DeveloperStore.Api.DependencyInjections;

public static class ServicesInjections
{
    public static IServiceCollection AddServicesInjection(this IServiceCollection services)
    {
        return services
            .AddScoped<IDomainEventPublisher, DomainEventPublisher>();                
    }
}