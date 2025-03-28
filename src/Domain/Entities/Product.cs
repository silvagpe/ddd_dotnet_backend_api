using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities;

public class Product : Entity
{    
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public string Category { get; private set; }
    public string? ImageUrl { get; private set; }
    public Rating Rating { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Product() { }  // For EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Product(long id, string title, string description, Money price, string category, Rating rating, string? imageUrl = null)
    {
        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Rating = rating ?? throw new ArgumentNullException(nameof(rating));
        ImageUrl = imageUrl;
    }

    public void UpdateDetails(string title, string description, Money price, string category, string? imageUrl = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Category = category ?? throw new ArgumentNullException(nameof(category));
        ImageUrl = imageUrl;
    }

    public void UpdateRating(Rating rating)
    {
        Rating = rating ?? throw new ArgumentNullException(nameof(rating));
    }
}