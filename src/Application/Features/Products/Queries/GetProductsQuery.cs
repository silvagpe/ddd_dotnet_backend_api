using DeveloperStore.Application.Common.Queries;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Application.Models;
using MediatR;

namespace Namespace.Application.Features.Products.Queries;

public class GetProductsQuery : BasePagedQuery, IRequest<PagedResult<ProductDto>>
{    
    public GetProductsQuery(int page, int pageSize, string? order, Dictionary<string, string> fields):base(page, pageSize, order, fields)
    {
    }
}