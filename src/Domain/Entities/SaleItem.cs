using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities;

public class SaleItem : Entity
{
    // External Identity pattern with denormalized product details
    public long ProductId { get; private set; }
    public Product Product { get; private set; }

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public Discount Discount { get; private set; }
    public decimal TotalPrice { get; private set; }

    // Navigation property - EF Core
    public long SaleId { get; private set; }
    public Sale? Sale { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private SaleItem() { }  // For EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public SaleItem(long id, Product product, int quantity, decimal unitPrice)
    {
        Id = id;
        if (quantity <= 0)
            throw new BusinessRuleException("Quantity must be greater than zero");

        if (quantity > 20)
            throw new BusinessRuleException("Cannot sell more than 20 identical items");

        Product = product ?? throw new ArgumentNullException(nameof(product));
        ProductId = product.Id;
        Quantity = quantity;
        UnitPrice = unitPrice == 0 ? throw new ArgumentNullException(nameof(unitPrice)) : unitPrice;
        Discount = CalculateDiscount(quantity);
        TotalPrice = CalculateTotalPrice();
    }

    private Discount CalculateDiscount(int quantity)
    {
        if (quantity >= 10 && quantity <= 20)
            return Discount.Twenty;
        else if (quantity >= 4)
            return Discount.Ten;
        else
            return Discount.None;
    }

    private decimal CalculateTotalPrice()
    {
        var totalBeforeDiscount = UnitPrice *  Quantity;
        return Discount.ApplyTo(totalBeforeDiscount);
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new BusinessRuleException("Quantity must be greater than zero");

        if (quantity > 20)
            throw new BusinessRuleException("Cannot sell more than 20 identical items");

        Quantity = quantity;
        Discount = CalculateDiscount(quantity);
        TotalPrice = CalculateTotalPrice();
    }
}