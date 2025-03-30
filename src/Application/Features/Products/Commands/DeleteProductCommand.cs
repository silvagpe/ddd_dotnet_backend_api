using DeveloperStore.Application.Features.Products.Dtos;
using MediatR;

namespace DeveloperStore.Application.Features.Products.Commands;

public class DeleteProductCommand: IRequest<string>
{
    public long Id { get; set; }

    public DeleteProductCommand(long id)
    {
        Id = id;
    }
}