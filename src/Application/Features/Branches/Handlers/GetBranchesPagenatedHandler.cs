using DeveloperStore.Application.Features.Branches.Queries;
using MediatR;
using DeveloperStore.Application.Models;
using AutoMapper;
using DeveloperStore.Infrastructure.Data;
using DeveloperStore.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Application.Features.Branches.Dtos;

namespace DeveloperStore.Application.Features.Branches.Handlers;

public class GetBranchesPagenatedHandler : IRequestHandler<GetBranchesQuery, PagedResult<BranchDto>>
{    
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetBranchesPagenatedHandler(IMapper mapper, AppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<PagedResult<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {

        var query = _dbContext.Branches.AsQueryable();

        query = query.ApplyFilters(request.Fields);
            
        if (!string.IsNullOrEmpty(request.Order))
        {
            query = query.OrderByDynamic(request.Order);
        }

        // Paginação
        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var branches = _mapper.Map<IEnumerable<BranchDto>>(data);
        return new PagedResult<BranchDto>(
            branches,
            totalItems,
            request.Page,
            request.PageSize
        );
    }
}

