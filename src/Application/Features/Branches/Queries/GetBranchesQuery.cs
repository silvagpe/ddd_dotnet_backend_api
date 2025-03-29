using DeveloperStore.Application.Common.Queries;
using DeveloperStore.Application.Features.Branches.Dtos;
using DeveloperStore.Application.Models;
using MediatR;

namespace DeveloperStore.Application.Features.Branches.Queries;

public class GetBranchesQuery : BasePagedQuery, IRequest<PagedResult<BranchDto>>
{    
    public GetBranchesQuery(int page, int pageSize, string? order, Dictionary<string, string> fields):base(page, pageSize, order, fields)
    {
    }
}