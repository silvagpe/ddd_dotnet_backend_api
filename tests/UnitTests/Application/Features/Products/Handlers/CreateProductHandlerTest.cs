using AutoMapper;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Application.Features.Products.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.ValueObjects;
using MediatR;
using NSubstitute;
using SharpAbp.Abp.Snowflakes;
using Xunit;

namespace DeveloperStore.Application.Features.Products.Handlers.Tests;

public class CreateProductHandlerTest
{
    private readonly IProductRepository _productRepositoryMock;
    private readonly IMapper _mapperMock;
    private readonly Snowflake _snowflakeMock;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTest()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _mapperMock = Substitute.For<IMapper>();
        _snowflakeMock = new Snowflake(workerId: 1, datacenterId: 1);
        _handler = new CreateProductHandler(_productRepositoryMock, _mapperMock, _snowflakeMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductDto_WhenProductIsCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateProductCommand { Title = "Test Product", Price = 100 };
        var productId = 123456789L;
        var product = new Product(productId, "Test Product", description: "Descr Test", 100M, "category", new Rating(5, 10), image: null);         
        var productDto = new ProductDto { Id = productId, Title = "Test Product", Price = 100 };
        
        _mapperMock.Map<CreateProductCommand, Product>(command).Returns(product);
        _productRepositoryMock.AddAsync(product, Arg.Any<CancellationToken>()).Returns(product);
        _mapperMock.Map<Product, ProductDto>(product).Returns(productDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productDto.Id, result.Id);
        Assert.Equal(productDto.Title, result.Title);
        Assert.Equal(productDto.Price, result.Price);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductCreationFails()
    {
        // Arrange
        var command = new CreateProductCommand { Title = "Test Product", Price = 100 };
        var productId = 123456789L;
        var product = new Product(productId, "Test Product", description: "Descr Test", 100M, "category", new Rating(5, 10), image: null);      
        
        _mapperMock.Map<CreateProductCommand, Product>(command).Returns(product);
        _productRepositoryMock.AddAsync(product, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}