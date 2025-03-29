namespace DeveloperStore.Application.Features.Products.Mapping;

using AutoMapper;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl));

        CreateMap<Rating, RatingDto>();

        CreateMap<decimal, Money>()
            .ConvertUsing(src => new Money(src, "USD"));

        CreateMap<Money, decimal>()
            .ConvertUsing(src => src.Value); 

        CreateMap<CreateProductCommand, Product>()     
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            // .ForMember(dest => dest.ImageUrl, opt =>
            // {
            //     opt.MapFrom(src =>
            //     {
            //         Console.WriteLine($"Mapping Image: {src.Image}"); // Log para depuração
            //         return src.Image;
            //     });
            // });
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image));
            
        CreateMap<RatingDto, Rating>();
    }
}