using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class SaleItemTest
{
    private readonly Product _product;
    public SaleItemTest()
    {
        _product = new Product(1, "Test Product", "Test Description", new Money(50), "Category", null);        
    }

    [Fact]
    public void Constructor_ShouldCreateSaleItem_WhenValidParametersAreProvided()
    {
        // Arrange    
        var unitPrice = new Money(100);

        // Act
        var saleItem = new SaleItem(1, _product, 5, unitPrice);

        // Assert
        Assert.Equal(1, saleItem.Id);
        Assert.Equal(_product, saleItem.Product);
        Assert.Equal(5, saleItem.Quantity);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.Equal(Discount.Ten, saleItem.Discount);
        Assert.Equal(new Money(450), saleItem.TotalPrice); // 10% discount applied
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenQuantityIsZeroOrNegative()
    {
        // Arrange        
        var unitPrice = new Money(100);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => new SaleItem(1, _product, 0, unitPrice));
        Assert.Throws<BusinessRuleException>(() => new SaleItem(1, _product, -1, unitPrice));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenQuantityExceedsLimit()
    {
        // Arrange        
        var unitPrice = new Money(100);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => new SaleItem(1, _product, 21, unitPrice));
    }

    [Fact]
    public void UpdateQuantity_ShouldUpdateQuantityAndRecalculateDiscountAndTotalPrice()
    {
        // Arrange
        var unitPrice = new Money(100);
        var saleItem = new SaleItem(1, _product, 5, unitPrice);

        // Act
        saleItem.UpdateQuantity(10);

        // Assert
        Assert.Equal(10, saleItem.Quantity);
        Assert.Equal(Discount.Twenty, saleItem.Discount);
        Assert.Equal(new Money(800), saleItem.TotalPrice); // 20% discount applied
    }

    [Fact]
    public void UpdateQuantity_ShouldThrowException_WhenQuantityIsZeroOrNegative()
    {
        // Arrange
        var unitPrice = new Money(100);
        var saleItem = new SaleItem(1, _product, 5, unitPrice);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => saleItem.UpdateQuantity(0));
        Assert.Throws<BusinessRuleException>(() => saleItem.UpdateQuantity(-1));
    }

    [Fact]
    public void UpdateQuantity_ShouldThrowException_WhenQuantityExceedsLimit()
    {
        // Arrange
        var unitPrice = new Money(100);
        var saleItem = new SaleItem(1, _product, 5, unitPrice);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => saleItem.UpdateQuantity(21));
    }
}