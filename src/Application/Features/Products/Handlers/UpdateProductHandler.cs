using AutoMapper;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.ValueObjects;
using MediatR;
using SharpAbp.Abp.Snowflakes;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;        
    }

    public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {        
        var product =  _mapper.Map<UpdateProductCommand, Product>(request);        

        product =  await _productRepository.UpdateAsync(product, cancellationToken);

        if (product is null)
        {
            return null;
        }   

        return _mapper.Map<Product, ProductDto>(product);
    }
}
