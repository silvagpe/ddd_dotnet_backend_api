using AutoMapper;
using DeveloperStore.Application.Features.Carts.Dtos;
using DeveloperStore.Application.Features.Carts.Queries;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Application.Features.Carts.Handlers;
public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, SaleDto?>
{
    private readonly ISaleRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartByIdHandler(ISaleRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;        
    }

    public async Task<SaleDto?> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {                
        var sale = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null)
            return null;

        var saleProductsDto = sale.Items.Select(_mapper.Map<SaleProductDto>).ToList();        
        var saleDto =  _mapper.Map<Sale, SaleDto>(sale);

        saleDto.Products = saleProductsDto;
        return saleDto;  
    }
}