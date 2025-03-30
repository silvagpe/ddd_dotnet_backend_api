using AutoMapper;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Application.Features.Products.Queries;
using DeveloperStore.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Application.Features.Products.Handlers;



public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;        
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {                
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        return product is null ? null : _mapper.Map<ProductDto>(product);        
    }
}