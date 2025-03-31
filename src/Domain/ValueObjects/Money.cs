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

    public static Money FromString(string  value, string currency = "USD")
    {
        if (!decimal.TryParse(value, out var parsedValue))
            throw new BusinessRuleException("Invalid money value format");
            
        return new Money(parsedValue, currency);
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

    public static bool operator >=(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare Money with different currencies.");

        return left.Value >= right.Value;
    }

    public static bool operator <=(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare Money with different currencies.");

        return left.Value <= right.Value;
    }

    public static bool operator >(Money left, Money right) => left >= right && left != right;
    public static bool operator <(Money left, Money right) => left <= right && left != right;

    public override bool Equals(object obj)
    {
        if (obj is Money other)
            return Value == other.Value && Currency == other.Currency;

        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Value, Currency);
}