using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void CreateMoney_WithValidValue_ShouldSucceed()
    {
        var money = new Money(100, "USD");
        Assert.Equal(100, money.Value);
        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void CreateMoney_WithNegativeValue_ShouldThrowException()
    {
        Assert.Throws<BusinessRuleException>(() => new Money(-1, "USD"));
    }

    [Fact]
    public void AddMoney_WithSameCurrency_ShouldSucceed()
    {
        var money1 = new Money(50, "USD");
        var money2 = new Money(30, "USD");

        var result = money1.Add(money2);

        Assert.Equal(80, result.Value);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void AddMoney_WithDifferentCurrencies_ShouldThrowException()
    {
        var money1 = new Money(50, "USD");
        var money2 = new Money(30, "EUR");

        Assert.Throws<BusinessRuleException>(() => money1.Add(money2));
    }

    [Fact]
    public void SubtractMoney_WithSameCurrency_ShouldSucceed()
    {
        var money1 = new Money(50, "USD");
        var money2 = new Money(30, "USD");

        var result = money1.Subtract(money2);

        Assert.Equal(20, result.Value);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void SubtractMoney_WithDifferentCurrencies_ShouldThrowException()
    {
        var money1 = new Money(50, "USD");
        var money2 = new Money(30, "EUR");

        Assert.Throws<BusinessRuleException>(() => money1.Subtract(money2));
    }

    [Fact]
    public void MultiplyMoney_ShouldSucceed()
    {
        var money = new Money(50, "USD");

        var result = money.Multiply(2);

        Assert.Equal(100, result.Value);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void ZeroMoney_ShouldReturnZeroValue()
    {
        var zeroMoney = Money.Zero("USD");

        Assert.Equal(0, zeroMoney.Value);
        Assert.Equal("USD", zeroMoney.Currency);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        var money = new Money(100.50m, "USD");

        Assert.Equal("100,50 USD", money.ToString());
    }
}
