using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;
using SharpAbp.Abp.Snowflakes;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class ProductTests
{

    private readonly Snowflake _snowflakeIdGenerator;

    public ProductTests()
    {
        _snowflakeIdGenerator = new Snowflake(workerId: 1, datacenterId: 1);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var price = 100;
        var product = new Product(_snowflakeIdGenerator.NextId(), "ProductName", "Description", 100, "Category", new Rating(5,100), "ImageUrl");

        Assert.Equal("ProductName", product.Title);
        Assert.Equal("Description", product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal("Category", product.Category);
        Assert.Equal("ImageUrl", product.ImageUrl);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var price = 100;
        var product = new Product(_snowflakeIdGenerator.NextId(), "ProductName", "Description", price, "Category", new Rating(4.5,70),"ImageUrl");

        product.UpdateDetails("NewName", "NewDescription", 200, "NewCategory", "NewImageUrl");

        Assert.Equal("NewName", product.Title);
        Assert.Equal("NewDescription", product.Description);
        Assert.Equal(200, product.Price);
        Assert.Equal("NewCategory", product.Category);
        Assert.Equal("NewImageUrl", product.ImageUrl);
    }
}
