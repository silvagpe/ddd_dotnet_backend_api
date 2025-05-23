using DeveloperStore.Application.Features.Branches.Dtos;
using MediatR;

namespace DeveloperStore.Application.Queries;

public class GetBranchByIdQuery: IRequest<BranchDto?>
{
    public long BranchId { get; }

    public GetBranchByIdQuery(long branchId)
    {
        BranchId = branchId;
    }
}