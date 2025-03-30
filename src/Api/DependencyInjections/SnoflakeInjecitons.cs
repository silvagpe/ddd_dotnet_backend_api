using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Repositories;
using SharpAbp.Abp.Snowflakes;

namespace DeveloperStore.Api.DependencyInjections;

public static class SnoflakeInjecitons
{
    public static IServiceCollection AddSnowflakeInjection(this IServiceCollection services)
    {
        return services.AddSingleton<Snowflake>();        
    }
}