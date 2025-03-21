using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities;

public class Product : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public string Category { get; private set; }
    public string ImageUrl { get; private set; }

    private Product() { }  // For EF Core

    public Product(string name, string description, Money price, string category, string imageUrl = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Category = category ?? throw new ArgumentNullException(nameof(category));
        ImageUrl = imageUrl;
    }

    public void UpdateDetails(string name, string description, Money price, string category, string imageUrl = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Category = category ?? throw new ArgumentNullException(nameof(category));
        ImageUrl = imageUrl;
    }
}