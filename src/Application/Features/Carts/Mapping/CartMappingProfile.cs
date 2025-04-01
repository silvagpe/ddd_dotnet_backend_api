using AutoMapper;

namespace DeveloperStore.Application.Features.Carts.Mapping;



public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<Commands.CartProduct, Domain.Entities.SaleItem>()            
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            
        CreateMap<Commands.CreateCartCommand, Domain.Entities.Sale>()            
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => DateTime.Parse(src.Date.ToString())))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));


        CreateMap<Domain.Entities.SaleItem, Dtos.SaleProductDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        
        CreateMap<Domain.Entities.Sale, Dtos.SaleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.SaleDate.ToString("yyyy-MM-dd")));
    }
}