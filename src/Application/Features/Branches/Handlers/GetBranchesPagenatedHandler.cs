using DeveloperStore.Application.Features.Branches.Queries;
using MediatR;
using DeveloperStore.Application.Models;
using AutoMapper;
using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Application.Features.Branches.Dtos;
using DeveloperStore.Domain.Repositories;

namespace DeveloperStore.Application.Features.Branches.Handlers;

public class GetBranchesPagenatedHandler : IRequestHandler<GetBranchesQuery, PagedResult<BranchDto>>
{    
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public GetBranchesPagenatedHandler(IMapper mapper, IBranchRepository branchRepository)
    {
        _mapper = mapper;
        _branchRepository = branchRepository;
    }

    public async Task<PagedResult<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var result = await _branchRepository.GetAllAsync(
            cancellationToken,
            request.Fields,
            request.Order,
            request.Page,
            request.PageSize
        );
            
        return new PagedResult<BranchDto>(
            _mapper.Map<IEnumerable<BranchDto>>(result.Branches),
            result.TotalItems,
            request.Page,
            request.PageSize
        );
    }
}

