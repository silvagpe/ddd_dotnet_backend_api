using DeveloperStore.Application.Features.Products.Dtos;
using MediatR;

namespace DeveloperStore.Application.Features.Products.Queries;

public class GetProductByIdQuery: IRequest<ProductDto?>
{
    public long ProductId { get; }

    public GetProductByIdQuery(long productId)
    {
        ProductId = productId;
    }
}