using AutoMapper;
using DeveloperStore.Application.Dtos;
using DeveloperStore.Application.Queries;
using DeveloperStore.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Application.Handlers;

public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, BranchDto?>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public GetBranchByIdQueryHandler(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<BranchDto?> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        return branch is null ? null : _mapper.Map<BranchDto>(branch);
    }
}