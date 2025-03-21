using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;

namespace DeveloperStore.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Value { get; private set; }
    public string Currency { get; private set; } = "USD";  // Default currency

    private Money() { }  // For EF Core

    public Money(decimal value, string currency = "USD")
    {
        if (value < 0)
            throw new BusinessRuleException("Money amount cannot be negative");

        Value = value;
        Currency = currency;
    }

    public static Money FromDecimal(decimal value, string currency = "USD")
    {
        return new Money(value, currency);
    }

    public static Money Zero(string currency = "USD")
    {
        return new Money(0, currency);
    }

    public Money Add(Money money)
    {
        if (Currency != money.Currency)
            throw new BusinessRuleException("Cannot add money with different currencies");

        return new Money(Value + money.Value, Currency);
    }

    public Money Subtract(Money money)
    {
        if (Currency != money.Currency)
            throw new BusinessRuleException("Cannot subtract money with different currencies");

        return new Money(Value - money.Value, Currency);
    }

    public Money Multiply(decimal multiplier)
    {
        return new Money(Value * multiplier, Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }

    public override string ToString()
    {
        return $"{Value} {Currency}";
    }
}