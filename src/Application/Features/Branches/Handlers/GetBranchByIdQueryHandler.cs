using AutoMapper;
using DeveloperStore.Application.Dtos;
using DeveloperStore.Application.Queries;
using DeveloperStore.Domain.Repositories;

namespace DeveloperStore.Application.Handlers;

public class GetBranchByIdQueryHandler
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public GetBranchByIdQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<BranchDto?> HandleAsync(GetBranchByIdQuery query)
    {
        var branch = await _branchRepository.GetByIdAsync(query.BranchId);
        if (branch is null)
            return null;

        return _mapper.Map<BranchDto>(branch);
    }
}