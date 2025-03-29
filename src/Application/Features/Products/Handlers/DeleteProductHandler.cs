using AutoMapper;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.ValueObjects;
using MediatR;
using SharpAbp.Abp.Snowflakes;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public DeleteProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;        
    }

    public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {                
        var result = await _productRepository.DeleteAsync(request.Id, cancellationToken);
        return result ? "Product deleted successfully" : "Product not found";
    }
}
