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

public class CreateProductHandler : IRequestHandler<CreateCartCommand, SaleDto?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly Snowflake _snowflake;
    private readonly IMapper _mapper;

    public CreateProductHandler(ISaleRepository saleRepository, IMapper mapper, Snowflake snowflake, IProductRepository productRepository, ICustomerRepository customerRepository, IBranchRepository branchRepository, IDomainEventPublisher domainEventPublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _snowflake = snowflake;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<SaleDto?> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var validationFailure = new List<ValidationFailure>();
        var products = await _productRepository.GetByIdsAsync(request.Products.Select(p => p.ProductId).ToArray(), cancellationToken);
        if (products.Count == 0)
        {
            validationFailure.Add(new ValidationFailure("Products", "No products found with the given IDs."));
        }
        //TODO: improve tests to validate if the product is existing in the database.

        var customer = await _customerRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (customer is null)
        {
            validationFailure.Add(new ValidationFailure("Customer", "Customer not found."));
        }

        //TODO: Review branch information
        var branch = await _branchRepository.GetByIdAsync(1, cancellationToken);
        if (branch is null)
        {
            validationFailure.Add(new ValidationFailure("Branch", "Branch not found."));
        }

        if (validationFailure.Count > 0)
        {
            throw new ValidationException(validationFailure);
        }

        var sale = new Sale(_snowflake.NextId(), customer, branch, request.Date);
        foreach (var product in products)
        {
            var cartProduct = request.Products.FirstOrDefault(p => p.ProductId == product.Id);
            if (cartProduct != null)
            {
                sale.AddItem(_snowflake.NextId(), product, cartProduct.Quantity, product.Price);
            }
        }

        var saleAdded = await _saleRepository.AddAsync(sale, cancellationToken);

        // Publish domain events
        foreach (var domainEvent in sale.DomainEvents)
        {
            await _domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
        }

        // Clear domain events after publishing
        sale.ClearDomainEvents();

        var saleProductsDto = saleAdded.Items.Select(_mapper.Map<SaleProductDto>).ToList();        
        var saleDto =  _mapper.Map<Sale, SaleDto>(saleAdded);

        saleDto.Products = saleProductsDto;
        return saleDto;

    }
}
