using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Application.Features.Branches.Queries;
using MediatR;
using DeveloperStore.Application.Models;
using DeveloperStore.Application.Dtos;
using AutoMapper;

namespace DeveloperStore.Application.Features.Branches.Handlers;

public class GetAllPagenatedHandler: IRequestHandler<GetAllPagenatedQuery, PagedResult<BranchDto>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public GetAllPagenatedHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<BranchDto>> Handle(GetAllPagenatedQuery request, CancellationToken cancellationToken)
    {
        var result = await _branchRepository.GetAllAsync(request.Page, request.PageSize);

        var branches = _mapper.Map<IEnumerable<BranchDto>>(result.branches);

        return new PagedResult<BranchDto>(
            branches,
            result.totalCount,
            request.Page,
            request.PageSize
        );
       
    }
}

