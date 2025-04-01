using MediatR;
using DeveloperStore.Application.Models;
using AutoMapper;
using DeveloperStore.Domain.Repositories;
using Namespace.Application.Features.Carts.Queries;
using DeveloperStore.Application.Features.Carts.Dtos;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Application.Features.Carts.Handlers;

public class GetCartsPagenatedHandler : IRequestHandler<GetCartsQuery, PagedResult<SaleDto>>
{

    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetCartsPagenatedHandler(IMapper mapper, ISaleRepository saleRepository)
    {
        _mapper = mapper;
        _saleRepository = saleRepository;
    }

    public async Task<PagedResult<SaleDto>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var result = await _saleRepository.GetAllAsync(
            cancellationToken,
            request.Fields,
            request.Order,
            request.Page,
            request.PageSize
        );

        List<SaleDto> salesDto = new List<SaleDto>();
        foreach (var sale in result.Sales)
        {
            var saleProductsDto = sale.Items.Select(_mapper.Map<SaleProductDto>).ToList();
            var saleDto = _mapper.Map<Sale, SaleDto>(sale);

            saleDto.Products = saleProductsDto;
            salesDto.Add(saleDto);
        }

        return new PagedResult<SaleDto>(
            salesDto,
            result.TotalItems,
            request.Page,
            request.PageSize
        );
    }
}

