using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Repositories;

namespace DeveloperStore.Api.DependencyInjections;

public static class RepositoryInjections
{
    public static IServiceCollection AddRepositoryInjection(this IServiceCollection services)
    {
        return services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IBranchRepository, BranchRepository>()
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<ISaleRepository, SaleRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>();        
    }
}