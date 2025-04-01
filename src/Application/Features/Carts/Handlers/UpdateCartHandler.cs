using AutoMapper;
using DeveloperStore.Application.Features.Carts.Commands;
using DeveloperStore.Application.Features.Carts.Dtos;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Domain.Services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SharpAbp.Abp.Snowflakes;
using Volo.Abp;

namespace DeveloperStore.Application.Features.Carts.Handlers;

public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, SaleDto?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly Snowflake _snowflake;
    private readonly IMapper _mapper;

    public UpdateCartHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IDomainEventPublisher domainEventPublisher,
        Snowflake snowflake)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _domainEventPublisher = domainEventPublisher;
        _snowflake = snowflake;
    }

    public async Task<SaleDto?> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var validationFailure = new List<ValidationFailure>();
        var saleExist = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (saleExist is null)
        {
            validationFailure.Add(new ValidationFailure("Sale", "Sale not found."));
            throw new ValidationException(validationFailure);
        }

        var validProducts = await _productRepository.GetByIdsAsync(request.Products.Select(p => p.ProductId).ToArray(), cancellationToken);
        if (validProducts.Count == 0)
        {
            validationFailure.Add(new ValidationFailure("Products", "No products found with the given IDs."));
        }
        if (validationFailure.Count > 0)
        {
            throw new ValidationException(validationFailure);
        }

        var productsToRemove = saleExist.Items.Where(i => !validProducts.Any(p => p.Id == i.ProductId)).ToList();
        foreach (var product in productsToRemove)
        {
            saleExist.RemoveItem(product.Id);
        }

        var productsToUpdate = validProducts.Where(p => saleExist.Items.Any(i => i.ProductId == p.Id)).ToList();
        foreach (var product in productsToUpdate)
        {
            var cartProduct = request.Products.FirstOrDefault(p => p.ProductId == product.Id);
            if (cartProduct != null)
            {
                saleExist.UpdateItemQuantity(product.Id, cartProduct.Quantity);
            }
        }

        var productsToAdd = validProducts.Where(p => !saleExist.Items.Any(i => i.ProductId == p.Id)).ToList();
        foreach (var product in productsToAdd)
        {
            var cartProduct = request.Products.FirstOrDefault(p => p.ProductId == product.Id);
            if (cartProduct != null)
            {
                saleExist.AddItem(_snowflake.NextId(), product, cartProduct.Quantity, product.Price);
            }
        }

        await _saleRepository.UpdateAsync(saleExist, cancellationToken);

        // Publish domain events
        foreach (var domainEvent in saleExist.DomainEvents)
        {
            await _domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
        }

        // Clear domain events after publishing
        saleExist.ClearDomainEvents();

        var saleProductsDto = saleExist.Items.Select(_mapper.Map<SaleProductDto>).ToList();
        var saleDto = _mapper.Map<Sale, SaleDto>(saleExist);

        saleDto.Products = saleProductsDto;
        return saleDto;

    }
}
