using AutoMapper;
using DeveloperStore.Application.Features.Carts.Commands;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, string>
{
    private readonly ISaleRepository _saleRepository;

    public DeleteCartHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;        
    }

    public async Task<string> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {                
        var result = await _saleRepository.DeleteAsync(request.Id, cancellationToken);
        return result ? "Sale deleted successfully" : "Sale not found";
    }
}
