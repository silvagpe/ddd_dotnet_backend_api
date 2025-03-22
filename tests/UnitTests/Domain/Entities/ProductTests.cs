using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var price = new Money(100);
        var product = new Product("ProductName", "Description", price, "Category", "ImageUrl");

        Assert.Equal("ProductName", product.Name);
        Assert.Equal("Description", product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal("Category", product.Category);
        Assert.Equal("ImageUrl", product.ImageUrl);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var price = new Money(100);
        var product = new Product("ProductName", "Description", price, "Category", "ImageUrl");

        product.UpdateDetails("NewName", "NewDescription", new Money(200), "NewCategory", "NewImageUrl");

        Assert.Equal("NewName", product.Name);
        Assert.Equal("NewDescription", product.Description);
        Assert.Equal(new Money(200), product.Price);
        Assert.Equal("NewCategory", product.Category);
        Assert.Equal("NewImageUrl", product.ImageUrl);
    }
}
