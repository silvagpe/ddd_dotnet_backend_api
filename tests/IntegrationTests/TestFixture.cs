using System.Diagnostics;
using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace IntegrationTests;

public class TestFixture : IDisposable
{
    public PostgreSqlContainer PostgreSqlContainer { get; private set; }
    public string ConnectionString { get; private set; }
    public HttpClient Client { get; private set; }
    private WebApplicationFactory<Program> _factory;

    public TestFixture()
    {
        // Inicializa o container PostgreSQL
        PostgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("developerstore")
            .WithUsername("user")
            .WithPassword("password")
            .Build();

        PostgreSqlContainer.StartAsync().GetAwaiter().GetResult();
        ConnectionString = PostgreSqlContainer.GetConnectionString();
        Debug.WriteLine($"PostgreSQL connection string: {ConnectionString}");

        // Configura o WebApplicationFactory para subir a API
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Substitui o DbContext para usar o container PostgreSQL
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseNpgsql(ConnectionString);
                    });
                });
            });

        // Cria o HttpClient para realizar chamadas à API
        Client = _factory.CreateClient();
    }

    public void Dispose()
    {
        Client?.Dispose();
        // _factory?.Dispose();
        PostgreSqlContainer?.DisposeAsync().GetAwaiter().GetResult();
    }
}