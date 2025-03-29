using DeveloperStore.Application.Features.Branches.Dtos;
using DeveloperStore.Application.Models;
using MediatR;

namespace DeveloperStore.Application.Features.Branches.Queries;

public class GetBranchesQuery : IRequest<PagedResult<BranchDto>>
{
    public int Page { get; }
    public int PageSize { get; }
    public string? Order { get; }
    public Dictionary<string, string> Fields { get; }

    public GetBranchesQuery(int page, int pageSize, string? order, Dictionary<string, string> fields)
    {
        Page = page;
        PageSize = pageSize;
        Order = order;
        Fields = fields;
    }
}