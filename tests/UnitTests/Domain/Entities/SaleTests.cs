using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using FluentAssertions;
using SharpAbp.Abp.Snowflakes;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class SaleTest
{
    private readonly Customer _customer;
    private readonly Branch _branch;
    private readonly Product _product1;
    private readonly Product _product2;

    private readonly Snowflake _snowflakeIdGenerator;
    
    public SaleTest()
    {
        _snowflakeIdGenerator =  new Snowflake(workerId: 1, datacenterId: 1);
        _customer = new Customer(_snowflakeIdGenerator.NextId(), "John", "Doe", "john@example.com", "1234567890");
        _branch = new Branch(_snowflakeIdGenerator.NextId(), "Main Branch", "123 Main St", "City", "State", "99123", "12345");
        _product1 = new Product(_snowflakeIdGenerator.NextId(), "Product 1", "Description 1", Money.FromDecimal(10.99m), "Software", new Rating(4.5, 100));
        _product2 = new Product(_snowflakeIdGenerator.NextId(), "Product 2", "Description 2", Money.FromDecimal(20.50m),  "Hardware", new Rating(4.0, 50));
    }

    [Fact]
    public void Constructor_ShouldCreateValidSale()
    {
        // Arrange & Act
        var saleDate = DateTime.Now;
        var sale = new Sale(_snowflakeIdGenerator.NextId(),_customer, _branch, saleDate);

        // Assert
        sale.SaleNumber.Should().NotBeNullOrWhiteSpace();
        sale.SaleDate.Should().Be(saleDate);
        sale.CustomerId.Should().Be(_customer.Id);
        sale.BranchId.Should().Be(_branch.Id);
        sale.Status.Should().Be(SaleStatus.Created);
        sale.TotalAmount.Should().Be(Money.Zero());
        sale.Items.Should().BeEmpty();
        sale.DomainEvents.Should().ContainSingle(e => e is SaleCreatedEvent);
    }

    [Fact]
    public void AddItem_ShouldAddItemAndRecalculateTotal()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(),_customer, _branch, DateTime.Now);
        var unitPrice = Money.FromDecimal(15.00m);

        // Act
        sale.AddItem(_snowflakeIdGenerator.NextId(), _product1, 2, unitPrice);

        // Assert
        sale.Items.Should().HaveCount(1);
        sale.Items.First().ProductId.Should().Be(_product1.Id);
        sale.Items.First().Quantity.Should().Be(2);
        sale.Items.First().UnitPrice.Should().Be(unitPrice);
        sale.TotalAmount.Should().Be(Money.FromDecimal(30.00m));
        sale.DomainEvents.Should().Contain(e => e is SaleModifiedEvent);
    }

    [Fact]
    public void AddItem_WithExistingProduct_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(),_customer, _branch, DateTime.Now);
        sale.AddItem(_snowflakeIdGenerator.NextId(), _product1, 1, Money.FromDecimal(10.00m));

        // Act & Assert
        Action act = () => sale.AddItem(_snowflakeIdGenerator.NextId(), _product1, 2, Money.FromDecimal(10.00m));
        act.Should().Throw<BusinessRuleException>()
           .WithMessage($"Product {_product1.Title} is already in the sale. Update the quantity instead.");
    }

    [Fact]
    public void UpdateItemQuantity_ShouldUpdateQuantityAndRecalculateTotal()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(),_customer, _branch, DateTime.Now);
        sale.AddItem(_snowflakeIdGenerator.NextId(), _product1, 1, Money.FromDecimal(10.00m));
        var itemId = sale.Items.First().Id;

        // Act
        sale.UpdateItemQuantity(itemId, 3);

        // Assert
        sale.Items.First().Quantity.Should().Be(3);
        sale.TotalAmount.Should().Be(Money.FromDecimal(30.00m));
        sale.DomainEvents.Should().Contain(e => e is SaleModifiedEvent);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveItemAndRecalculateTotal()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(), _customer, _branch, DateTime.Now);
        sale.AddItem(_snowflakeIdGenerator.NextId(),_product1, 2, Money.FromDecimal(10.00m));
        sale.AddItem(_snowflakeIdGenerator.NextId(), _product2, 1, Money.FromDecimal(20.00m));

        // Act
        sale.RemoveItem(_product1.Id);

        // Assert
        sale.Items.Should().HaveCount(1);
        sale.Items.First().ProductId.Should().Be(_product2.Id);
        sale.TotalAmount.Should().Be(Money.FromDecimal(20.00m));
        sale.DomainEvents.Should().Contain(e => e is ItemCancelledEvent);
        sale.DomainEvents.Should().Contain(e => e is SaleModifiedEvent);
    }

    [Fact]
    public void CompleteSale_ShouldChangeStatusToCompleted()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(),_customer, _branch, DateTime.Now);
        sale.AddItem(_snowflakeIdGenerator.NextId(),_product1, 2, Money.FromDecimal(10.00m));

        // Act
        sale.CompleteSale();

        // Assert
        sale.Status.Should().Be(SaleStatus.Completed);
        sale.DomainEvents.Should().Contain(e => e is SaleModifiedEvent);
    }

    [Fact]
    public void CompleteSale_WithNoItems_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(),_customer, _branch, DateTime.Now);

        // Act & Assert
        Action act = () => sale.CompleteSale();
        act.Should().Throw<BusinessRuleException>()
           .WithMessage("Cannot complete a sale with no items");
    }

    [Fact]
    public void CancelSale_ShouldChangeStatusToCancelled()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(),_customer, _branch, DateTime.Now);

        // Act
        sale.CancelSale();

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.DomainEvents.Should().Contain(e => e is SaleCancelledEvent);
    }

    [Fact]
    public void CancelSale_WhenCompleted_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(), _customer, _branch, DateTime.Now);
        sale.AddItem(_snowflakeIdGenerator.NextId(), _product1, 1, Money.FromDecimal(10.00m));
        sale.CompleteSale();

        // Act & Assert
        Action act = () => sale.CancelSale();
        act.Should().Throw<BusinessRuleException>()
           .WithMessage("Cannot cancel a completed sale");
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var sale = new Sale(_snowflakeIdGenerator.NextId(), _customer, _branch, DateTime.Now);
        sale.AddItem(_snowflakeIdGenerator.NextId(), _product1, 1, Money.FromDecimal(10.00m));

        // Act
        sale.ClearDomainEvents();

        // Assert
        sale.DomainEvents.Should().BeEmpty();
    }
}