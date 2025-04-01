using MediatR;
using DeveloperStore.Application.Models;
using AutoMapper;
using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Application.Features.Products.Queries;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Domain.Repositories;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class GetProductsByCategoryPagenatedHandler : IRequestHandler<GetProductsByCategoryQuery, PagedResult<ProductDto>>
{

    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsByCategoryPagenatedHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {        
        if (!string.IsNullOrEmpty(request.Category))
        {
            request.Fields.Add("Category", request.Category);
        }

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

