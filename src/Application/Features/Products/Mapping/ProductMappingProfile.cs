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
        CreateMap<Product, ProductDto>();
        CreateMap<Rating, RatingDto>();

        CreateMap<CreateProductCommand, Product>()            
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Money.FromDecimal(src.Price, "USD")));
            
        CreateMap<RatingDto, Rating>();
    }
}