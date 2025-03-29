using DeveloperStore.Application.Common.Queries;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Application.Models;
using MediatR;

namespace DeveloperStore.Application.Features.Products.Queries;

public class GetProductsByCategoryQuery : BasePagedQuery, IRequest<PagedResult<ProductDto>>
{
    public string? Category { get; set; }
    public GetProductsByCategoryQuery(string? category, int page, int pageSize, string? order, Dictionary<string, string> fields)
        : base(page, pageSize, order, fields)
    {
        this.Category = category;
    }
}
