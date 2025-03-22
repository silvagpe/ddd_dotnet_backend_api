using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities;

public class SaleItem : Entity
{
    // External Identity pattern with denormalized product details
    public string ProductName { get; private set; }
    public string ProductCategory { get; private set; }

    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Discount Discount { get; private set; }
    public Money TotalPrice { get; private set; }

    // Navigation property - EF Core
    public int SaleId { get; private set; }

    private SaleItem() { }  // For EF Core

    public SaleItem(int productId, string productName, string productCategory, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new BusinessRuleException("Quantity must be greater than zero");

        if (quantity > 20)
            throw new BusinessRuleException("Cannot sell more than 20 identical items");

        Id = productId;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        ProductCategory = productCategory ?? throw new ArgumentNullException(nameof(productCategory));
        Quantity = quantity;
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
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

    private Money CalculateTotalPrice()
    {
        var totalBeforeDiscount = UnitPrice.Multiply(Quantity);
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