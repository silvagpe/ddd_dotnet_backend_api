using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Api.DependencyInjections;
using DeveloperStore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextInjection(builder.Configuration);
builder.Services.AddRepositoryInjection();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
