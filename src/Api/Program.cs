using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Api.DependencyInjections;
using DeveloperStore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContextInjection(builder.Configuration);
builder.Services.AddRepositoryInjection();
builder.Services.AddMediatR();
builder.Services.AddAutoMapperConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

//Automatic apply migrations.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Substitua pelo nome do seu DbContext
    dbContext.Database.Migrate();
}

app.Run();