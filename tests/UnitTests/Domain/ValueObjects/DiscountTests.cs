using System;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.ValueObjects;

public class DiscountTests
{
    [Fact]
    public void Constructor_ShouldCreateDiscount_WhenPercentageIsValid()
    {
        // Arrange
        decimal validPercentage = 0.2m;

        // Act
        var discount = new Discount(validPercentage);

        // Assert
        Assert.Equal(validPercentage, discount.Percentage);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Constructor_ShouldThrowException_WhenPercentageIsInvalid(decimal invalidPercentage)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => new Discount(invalidPercentage));
    }

    [Fact]
    public void ApplyTo_ShouldReturnDiscountedAmount_WhenCalled()
    {
        // Arrange
        var discount = new Discount(0.1m);
        var originalAmount = 100m;

        // Act
        var discountedAmount = discount.ApplyTo(originalAmount);

        // Assert
        Assert.Equal(90m, discountedAmount);
    }

    [Fact]
    public void None_ShouldReturnDiscountWithZeroPercentage()
    {
        // Act
        var noDiscount = Discount.None;

        // Assert
        Assert.Equal(0, noDiscount.Percentage);
    }

    [Fact]
    public void ToString_ShouldReturnPercentageAsString()
    {
        // Arrange
        var discount = new Discount(0.15m);

        // Act
        var result = discount.ToString();

        // Assert
        decimal expected = 15.00m;
        Assert.Equal($"{expected}%", result);
    }
}
