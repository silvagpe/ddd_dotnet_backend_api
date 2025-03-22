using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities;

public enum SaleStatus
{
    Created,
    Completed,
    Cancelled
}

public class Sale : Entity, IAggregateRoot
{
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }

    // External Identity pattern with denormalized details
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; }    

    public int BranchId { get; private set; }
    public Branch Branch { get; private set; }    

    public SaleStatus Status { get; private set; }
    public Money TotalAmount { get; private set; }

    private readonly List<SaleItem> _items = new List<SaleItem>();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Sale() { }  // For EF Core

    public Sale(
        Customer customer,
        Branch branch,
        DateTime saleDate)
    {
        SaleNumber = GenerateSaleNumber();
        SaleDate = saleDate;
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        CustomerId = customer.Id;
        Branch = branch ?? throw new ArgumentNullException(nameof(branch));        
        BranchId = branch.Id;
        Status = SaleStatus.Created;
        TotalAmount = Money.Zero();

        _domainEvents.Add(new SaleCreatedEvent(this));
    }

    public void AddItem(Product product, int quantity, Money unitPrice)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (_items.Any(i => i.ProductId == product.Id))
            throw new BusinessRuleException($"Product {product.Name} is already in the sale. Update the quantity instead.");

        var saleItem = new SaleItem(product, quantity, unitPrice);
        _items.Add(saleItem);

        RecalculateTotalAmount();
        _domainEvents.Add(new SaleModifiedEvent(this));
    }

    public void UpdateItemQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.Id == productId);
        if (item == null)
            throw new BusinessRuleException($"Product with ID {productId} not found in this sale");

        item.UpdateQuantity(quantity);
        RecalculateTotalAmount();
        _domainEvents.Add(new SaleModifiedEvent(this));
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            throw new BusinessRuleException($"Product with ID {productId} not found in this sale");

        _items.Remove(item);
        RecalculateTotalAmount();
        _domainEvents.Add(new ItemCancelledEvent(this, productId));
        _domainEvents.Add(new SaleModifiedEvent(this));
    }

    public void CompleteSale()
    {
        if (Status == SaleStatus.Cancelled)
            throw new BusinessRuleException("Cannot complete a cancelled sale");

        if (!_items.Any())
            throw new BusinessRuleException("Cannot complete a sale with no items");

        Status = SaleStatus.Completed;
        _domainEvents.Add(new SaleModifiedEvent(this));
    }

    public void CancelSale()
    {
        if (Status == SaleStatus.Completed)
            throw new BusinessRuleException("Cannot cancel a completed sale");

        Status = SaleStatus.Cancelled;
        _domainEvents.Add(new SaleCancelledEvent(this));
    }

    private void RecalculateTotalAmount()
    {
        var total = Money.Zero();
        foreach (var item in _items)
        {
            total = total.Add(item.TotalPrice);
        }
        TotalAmount = total;
    }

    private string GenerateSaleNumber()
    {
        // Format: YYYYMMDD-GUID (first 8 chars)
        string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        string guidPart = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        return $"{datePart}-{guidPart}";
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}