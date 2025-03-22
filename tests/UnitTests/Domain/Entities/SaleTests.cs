using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var sale = new Sale(1, "CustomerName", "customer@example.com", 2, "BranchName", "BranchLocation", DateTime.UtcNow);

        Assert.Equal(1, sale.CustomerId);
        Assert.Equal("CustomerName", sale.CustomerName);
        Assert.Equal("customer@example.com", sale.CustomerEmail);
        Assert.Equal(2, sale.BranchId);
        Assert.Equal("BranchName", sale.BranchName);
        Assert.Equal("BranchLocation", sale.BranchLocation);
        Assert.Empty(sale.Items);
    }

    [Fact]
    public void AddItem_ShouldAddNewItem()
    {
        var sale = new Sale(1, "CustomerName", "customer@example.com", 2, "BranchName", "BranchLocation", DateTime.UtcNow);
        var unitPrice = new Money(100);

        sale.AddItem(1, "ProductName", "Category", 5, unitPrice);

        Assert.Single(sale.Items);
    }

    [Fact]
    public void CompleteSale_ShouldUpdateStatus()
    {
        var sale = new Sale(1, "CustomerName", "customer@example.com", 2, "BranchName", "BranchLocation", DateTime.UtcNow);
        var unitPrice = new Money(100);

        sale.AddItem(1, "ProductName", "Category", 5, unitPrice);
        sale.CompleteSale();

        Assert.Equal(SaleStatus.Completed, sale.Status);
    }

    [Fact]
    public void UpdateItemQuantity_ShouldUpdateQuantity_WhenItemExists()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, Money.FromDecimal(10, "USD"));

        // Act
        sale.UpdateItemQuantity(1, 5);

        // Assert
        var item = sale.Items.First(i => i.Id == 1);
        Assert.Equal(5, item.Quantity);
    }

    [Fact]
    public void UpdateItemQuantity_ShouldRecalculateTotalAmount_WhenQuantityIsUpdated()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, Money.FromDecimal(10, "USD"));

        // Act
        sale.UpdateItemQuantity(1, 5);

        // Assert
        Assert.Equal(Money.FromDecimal(45, "USD"), sale.TotalAmount);
    }

    [Fact]
    public void UpdateItemQuantity_ShouldThrowException_WhenItemDoesNotExist()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => sale.UpdateItemQuantity(1, 5));
        Assert.Equal("Product with ID 1 not found in this sale", exception.Message);
    }

    [Fact]
    public void UpdateItemQuantity_ShouldAddDomainEvent_WhenQuantityIsUpdated()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, Money.FromDecimal(10, "USD"));

        // Act
        sale.UpdateItemQuantity(1, 5);

        // Assert
        Assert.Contains(sale.DomainEvents, e => e is SaleModifiedEvent);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveItem_WhenItemExists()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, new Money(10, "USD"));
        sale.AddItem(2, "Product B", "Category B", 1, new Money(20, "USD"));

        // Act
        sale.RemoveItem(1);

        // Assert
        Assert.Single(sale.Items);
        Assert.DoesNotContain(sale.Items, i => i.Id == 1);
        Assert.Equal(new Money(20, "USD"), sale.TotalAmount);
        Assert.Contains(sale.DomainEvents, e => e is ItemCancelledEvent);
        Assert.Contains(sale.DomainEvents, e => e is SaleModifiedEvent);
    }

    [Fact]
    public void RemoveItem_ShouldThrowException_WhenItemDoesNotExist()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, new Money(10, "USD"));

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => sale.RemoveItem(2));
        Assert.Equal("Product with ID 2 not found in this sale", exception.Message);
    }

    [Fact]
    public void RemoveItem_ShouldRecalculateTotalAmount_WhenItemIsRemoved()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, new Money(10, "USD"));
        sale.AddItem(2, "Product B", "Category B", 1, new Money(20, "USD"));

        // Act
        sale.RemoveItem(2);

        // Assert
        Assert.Single(sale.Items);
        Assert.Equal(new Money(20, "USD"), sale.TotalAmount);
    }

    [Fact]
    public void CompleteSale_ShouldThrowException_WhenSaleIsCancelled()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.CancelSale();

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => sale.CompleteSale());
        Assert.Equal("Cannot complete a cancelled sale", exception.Message);
    }

    [Fact]
    public void CompleteSale_ShouldThrowException_WhenNoItemsInSale()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => sale.CompleteSale());
        Assert.Equal("Cannot complete a sale with no items", exception.Message);
    }

    [Fact]
    public void CompleteSale_ShouldSetStatusToCompleted_WhenValid()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, Money.FromDecimal(10, "USD"));

        // Act
        sale.CompleteSale();

        // Assert
        Assert.Equal(SaleStatus.Completed, sale.Status);
    }

    [Fact]
    public void CompleteSale_ShouldAddSaleModifiedEvent_WhenValid()
    {
        // Arrange
        var sale = new Sale(1, "John Doe", "john.doe@example.com", 1, "Main Branch", "123 Main St", DateTime.UtcNow);
        sale.AddItem(1, "Product A", "Category A", 2, Money.FromDecimal(10, "USD"));

        // Act
        sale.CompleteSale();

        // Assert
        Assert.Contains(sale.DomainEvents, e => e is SaleModifiedEvent);
    }

    [Fact]
    public void CancelSale_ShouldSetStatusToCancelled_WhenSaleIsNotCompleted()
    {
        // Arrange
        var sale = new Sale(
            customerId: 1,
            customerName: "John Doe",
            customerEmail: "john.doe@example.com",
            branchId: 1,
            branchName: "Main Branch",
            branchLocation: "123 Main St",
            saleDate: DateTime.UtcNow
        );

        // Act
        sale.CancelSale();

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.Contains(sale.DomainEvents, e => e is SaleCancelledEvent);
    }

    [Fact]
    public void CancelSale_ShouldThrowException_WhenSaleIsCompleted()
    {
        // Arrange
        var sale = new Sale(
            customerId: 1,
            customerName: "John Doe",
            customerEmail: "john.doe@example.com",
            branchId: 1,
            branchName: "Main Branch",
            branchLocation: "123 Main St",
            saleDate: DateTime.UtcNow
        );
        sale.AddItem(1, "Product A", "Category A", 2, Money.FromDecimal(10, "USD"));

        sale.CompleteSale();

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => sale.CancelSale());
        Assert.Equal("Cannot cancel a completed sale", exception.Message);
    }
}
