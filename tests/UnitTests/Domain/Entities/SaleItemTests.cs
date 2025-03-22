using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class SaleItemTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var unitPrice = new Money(100);
        var saleItem = new SaleItem(1, "ProductName", "Category", 5, unitPrice);

        Assert.Equal(1, saleItem.Id);
        Assert.Equal("ProductName", saleItem.ProductName);
        Assert.Equal("Category", saleItem.ProductCategory);
        Assert.Equal(5, saleItem.Quantity);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.NotNull(saleItem.TotalPrice);
    }

    [Fact]
    public void UpdateQuantity_ShouldRecalculateTotalPrice()
    {
        var unitPrice = new Money(100);
        var saleItem = new SaleItem(1, "ProductName", "Category", 5, unitPrice);

        saleItem.UpdateQuantity(10);

        Assert.Equal(10, saleItem.Quantity);
        Assert.NotNull(saleItem.TotalPrice);
    }

    [Theory]
    [InlineData(1, 0)]  // Quantity less than 4
    [InlineData(4, 10)]  // Quantity between 4 and 9
    [InlineData(10, 20)]  // Quantity between 10 and 20
    [InlineData(20, 20)]  // Maximum allowed quantity
    public void CalculateDiscount_ShouldReturnCorrectDiscount(int quantity, decimal expectedDiscount)
    {
        // Arrange
        var productId = 1;
        var productName = "Test Product";
        var productCategory = "Test Category";
        var unitPrice = new Money(100);

        Discount discount = new Discount(expectedDiscount/100);

        // Act
        var saleItem = new SaleItem(productId, productName, productCategory, quantity, unitPrice);

        // Assert
        Assert.Equal(discount, saleItem.Discount);
    }

    [Fact]
    public void CalculateDiscount_ShouldThrowException_WhenQuantityExceedsLimit()
    {
        // Arrange
        var productId = 1;
        var productName = "Test Product";
        var productCategory = "Test Category";
        var unitPrice = new Money(100);
        var quantity = 21; // Exceeds the limit

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() =>
            new SaleItem(productId, productName, productCategory, quantity, unitPrice));
    }

    [Fact]
    public void CalculateDiscount_ShouldThrowException_WhenQuantityIsZeroOrNegative()
    {
        // Arrange
        var productId = 1;
        var productName = "Test Product";
        var productCategory = "Test Category";
        var unitPrice = new Money(100);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() =>
            new SaleItem(productId, productName, productCategory, 0, unitPrice)); // Zero quantity

        Assert.Throws<BusinessRuleException>(() =>
            new SaleItem(productId, productName, productCategory, -1, unitPrice)); // Negative quantity
    }

    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var productId = 1;
        var productName = "Laptop";
        var productCategory = "Electronics";
        var quantity = 5;
        var unitPrice = new Money(1000);

        // Act
        var saleItem = new SaleItem(productId, productName, productCategory, quantity, unitPrice);

        // Assert
        Assert.Equal(productId, saleItem.Id);
        Assert.Equal(productName, saleItem.ProductName);
        Assert.Equal(productCategory, saleItem.ProductCategory);
        Assert.Equal(quantity, saleItem.Quantity);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.Equal(Discount.Ten, saleItem.Discount);
        Assert.Equal(new Money(4500), saleItem.TotalPrice); // 10% discount applied
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenQuantityIsZero()
    {
        // Arrange
        var productId = 1;
        var productName = "Laptop";
        var productCategory = "Electronics";
        var quantity = 0;
        var unitPrice = new Money(1000);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() =>
            new SaleItem(productId, productName, productCategory, quantity, unitPrice));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenQuantityExceedsLimit()
    {
        // Arrange
        var productId = 1;
        var productName = "Laptop";
        var productCategory = "Electronics";
        var quantity = 25;
        var unitPrice = new Money(1000);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() =>
            new SaleItem(productId, productName, productCategory, quantity, unitPrice));
    }

    [Fact]
    public void UpdateQuantity_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var saleItem = new SaleItem(1, "Laptop", "Electronics", 5, new Money(1000));
        var newQuantity = 10;

        // Act
        saleItem.UpdateQuantity(newQuantity);

        // Assert
        Assert.Equal(newQuantity, saleItem.Quantity);
        Assert.Equal(Discount.Twenty, saleItem.Discount);
        Assert.Equal(new Money(8000), saleItem.TotalPrice); // 20% discount applied
    }

    [Fact]
    public void UpdateQuantity_ShouldThrowException_WhenQuantityIsZero()
    {
        // Arrange
        var saleItem = new SaleItem(1, "Laptop", "Electronics", 5, new Money(1000));
        var newQuantity = 0;

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => saleItem.UpdateQuantity(newQuantity));
    }

    [Fact]
    public void UpdateQuantity_ShouldThrowException_WhenQuantityExceedsLimit()
    {
        // Arrange
        var saleItem = new SaleItem(1, "Laptop", "Electronics", 5, new Money(1000));
        var newQuantity = 25;

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => saleItem.UpdateQuantity(newQuantity));
    }

    [Fact]
    public void UpdateQuantity_ShouldUpdateQuantityAndRecalculateDiscountAndTotalPrice_WhenQuantityIsValid()
    {
        // Arrange
        var saleItem = new SaleItem(1, "Product A", "Category A", 5, new Money(10));
        int newQuantity = 8;

        // Act
        saleItem.UpdateQuantity(newQuantity);

        // Assert
        Assert.Equal(newQuantity, saleItem.Quantity);
        Assert.Equal(Discount.Ten, saleItem.Discount);
        Assert.Equal(new Money(72m), saleItem.TotalPrice);
    }

    [Fact]
    public void UpdateQuantity_ShouldThrowException_WhenQuantityIsZeroOrLess()
    {
        // Arrange
        var saleItem = new SaleItem(1, "Product A", "Category A", 5, new Money(10));
        int invalidQuantity = 0;

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => saleItem.UpdateQuantity(invalidQuantity));
        Assert.Equal("Quantity must be greater than zero", exception.Message);
    }

    [Fact]
    public void UpdateQuantity_ShouldThrowException_WhenQuantityExceedsMaximumLimit()
    {
        // Arrange
        var saleItem = new SaleItem(1, "Product A", "Category A", 5, new Money(10));
        int invalidQuantity = 21;

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => saleItem.UpdateQuantity(invalidQuantity));
        Assert.Equal("Cannot sell more than 20 identical items", exception.Message);
    }

    [Fact]
    public void UpdateQuantity_ShouldApplyCorrectDiscount_WhenQuantityIsBetween10And20()
    {
        // Arrange
        var saleItem = new SaleItem(1, "Product A", "Category A", 5, new Money(10));
        int newQuantity = 15;

        // Act
        saleItem.UpdateQuantity(newQuantity);

        // Assert
        Assert.Equal(newQuantity, saleItem.Quantity);
        Assert.Equal(Discount.Twenty, saleItem.Discount);
        Assert.Equal(new Money(120), saleItem.TotalPrice); // 15 * 10 with 20% discount
    }
}
