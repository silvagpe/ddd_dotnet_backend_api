using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Repositories;

namespace DeveloperStore.Api.DependencyInjections;

public static class RepositoryInjections
{
    public static IServiceCollection AddRepositoryInjection(this IServiceCollection services)
    {
        return services
            .AddScoped<IBranchRepository, BranchRepository>()
            .AddScoped<IProductRepository, ProductRepository>();
        
    }
}