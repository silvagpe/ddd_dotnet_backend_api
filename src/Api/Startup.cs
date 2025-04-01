using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Api.DependencyInjections;
using DeveloperStore.Api.Middleware;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddSwaggerGen();
        services.AddControllers();
        services.AddDbContextInjection(_configuration);
        services.AddRepositoryInjection();
        services.AddMediatR();
        services.AddAutoMapperConfig();
        services.AddSnowflakeInjection();
        services.AddServicesInjection();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // Automatic apply migrations.
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
