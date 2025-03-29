using MediatR;
using DeveloperStore.Application.Models;
using AutoMapper;
using DeveloperStore.Infrastructure.Data;
using DeveloperStore.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Application.Features.Products.Queries;
using DeveloperStore.Application.Features.Products.Dtos;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class GetProducsByCategoryPagenatedHandler : IRequestHandler<GetProductsByCategoryQuery, PagedResult<ProductDto>>
{

    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetProducsByCategoryPagenatedHandler(IMapper mapper, AppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {

        var query = _dbContext.Products.AsQueryable();
        if (!string.IsNullOrEmpty(request.Category))
        {
            query = query.Where(x => x.Category == request.Category);
        }

        query = query.ApplyFilters(request.Fields);
        
        if (!string.IsNullOrEmpty(request.Order))
        {
            query = query.OrderByDynamic(request.Order);
        }
        
        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var products = _mapper.Map<IEnumerable<ProductDto>>(data);
        return new PagedResult<ProductDto>(
            products,
            totalItems,
            request.Page,
            request.PageSize
        );
    }
}

