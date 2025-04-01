using DeveloperStore.Application.Features.Carts.Dtos;
using MediatR;

namespace DeveloperStore.Application.Features.Carts.Queries;

public class GetCartByIdQuery: IRequest<SaleDto?>
{
    public long Id { get; }

    public GetCartByIdQuery(long id)
    {
        Id = id;
    }
}