using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Application.Features.Branches.Queries;
using MediatR;
using DeveloperStore.Application.Models;
using DeveloperStore.Application.Dtos;
using AutoMapper;
using DeveloperStore.Infrastructure.Data;
using DeveloperStore.Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Application.Features.Branches.Handlers;

public class GetAllPagenatedHandler : IRequestHandler<GetBranchesQuery, PagedResult<BranchDto>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllPagenatedHandler(IBranchRepository branchRepository, IMapper mapper, AppDbContext dbContext)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<PagedResult<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {

        var query = _dbContext.Branches.AsQueryable();

        // Aplicar filtros dinâmicos
        foreach (var filter in request.Fields)
        {
            var key = filter.Key.ToLower();
            var value = filter.Value;

            if (key.StartsWith("_min"))
            {
                var field = key.Substring(4); // Remove o prefixo "_min"
                query = query.WhereDynamic(field, ">=", value);
            }
            else if (key.StartsWith("_max"))
            {
                var field = key.Substring(4); // Remove o prefixo "_max"
                query = query.WhereDynamic(field, "<=", value);
            }
            else
            {
                query = query.WhereDynamic(key, "=", value);
            }
        }

        // Aplicar ordenação
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


        // var result = await _branchRepository.GetAllAsync(request.Page, request.PageSize);
        // var totalCount = await _branchRepository.GetTotalCountAsync();

        // var branches = _mapper.Map<IEnumerable<BranchDto>>(result);

        // return new PagedResult<BranchDto>(
        //     branches,
        //     totalCount,
        //     request.Page,
        //     request.PageSize
        // );

    }
}

