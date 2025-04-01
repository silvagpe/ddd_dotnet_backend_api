using DeveloperStore.Application.Common.Queries;
using DeveloperStore.Application.Features.Carts.Dtos;
using DeveloperStore.Application.Models;
using MediatR;

namespace Namespace.Application.Features.Carts.Queries;

public class GetCartsQuery : BasePagedQuery, IRequest<PagedResult<SaleDto>>
{    
    public GetCartsQuery(int page, int pageSize, string? order, Dictionary<string, string> fields):base(page, pageSize, order, fields)
    {
    }
}