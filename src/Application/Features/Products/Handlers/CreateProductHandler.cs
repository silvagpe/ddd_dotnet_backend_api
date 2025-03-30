using AutoMapper;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.ValueObjects;
using MediatR;
using SharpAbp.Abp.Snowflakes;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly Snowflake _snowflake;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository productRepository, IMapper mapper, Snowflake snowflake)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _snowflake = snowflake;
    }

    public async Task<ProductDto?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        request.Id = _snowflake.NextId();
        var product =  _mapper.Map<CreateProductCommand, Product>(request);        

        product =  await _productRepository.AddAsync(product, cancellationToken);

        if (product is null)
        {
            return null;
        }   

        return _mapper.Map<Product, ProductDto>(product);
    }
}
