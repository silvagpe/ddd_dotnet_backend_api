namespace DeveloperStore.Application.Queries;

public class GetBranchByIdQuery
{
    public long BranchId { get; }

    public GetBranchByIdQuery(long branchId)
    {
        BranchId = branchId;
    }
}