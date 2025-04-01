using MediatR;
using DeveloperStore.Application.Models;
using AutoMapper;
using DeveloperStore.Application.Features.Products.Dtos;
using Namespace.Application.Features.Products.Queries;
using DeveloperStore.Domain.Repositories;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class GetProductsPagenatedHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{

    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsPagenatedHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetAllAsync(
            cancellationToken,
            request.Fields,
            request.Order,
            request.Page,
            request.PageSize
        );
            
        return new PagedResult<ProductDto>(
            _mapper.Map<IEnumerable<ProductDto>>(result.Products),
            result.TotalItems,
            request.Page,
            request.PageSize
        );
    }
}

