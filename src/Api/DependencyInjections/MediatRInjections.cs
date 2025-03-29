using System.Reflection;
using DeveloperStore.Application;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Application.Helpers;
using FluentValidation;
using MediatR;

namespace DeveloperStore.Api.DependencyInjections;

public static class MediatRInjections
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));  
        services.AddTransient<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddTransient<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
        services.AddTransient<IValidator<DeleteProductCommand>, DeleteProductCommandValidator>();
        services.AddTransient<IValidator<RatingDto>, RatingDtoValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));              
        return services;
    }
}