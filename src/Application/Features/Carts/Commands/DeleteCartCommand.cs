using DeveloperStore.Application.Features.Products.Dtos;
using MediatR;

namespace DeveloperStore.Application.Features.Carts.Commands;

public class DeleteCartCommand: IRequest<string>
{
    public long Id { get; set; }

    public DeleteCartCommand(long id)
    {
        Id = id;
    }
}