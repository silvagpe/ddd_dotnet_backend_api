using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;

namespace DeveloperStore.Domain.ValueObjects;

public class Discount : ValueObject
{
    public decimal Percentage { get; private set; }

    private Discount() { }  // For EF Core

    public Discount(decimal percentage)
    {
        if (percentage < 0 || percentage > 1)
            throw new BusinessRuleException("Discount percentage must be between 0 and 1");

        Percentage = percentage;
    }

    public decimal ApplyTo(decimal amount)
    {
        return amount * (1 - Percentage);
    }

    public static Discount None => new Discount(0);
    public static Discount Ten => new Discount(0.1m);
    public static Discount Twenty => new Discount(0.2m);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Percentage;
    }

    public override string ToString()
    {
        return $"{Percentage * 100}%";
    }
}