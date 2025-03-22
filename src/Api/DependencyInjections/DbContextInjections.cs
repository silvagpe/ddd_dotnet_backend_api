using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Api.DependencyInjections;

public static class DbContextInjections
{
    public static IServiceCollection AddDbContextInjection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));        
    }
    
}