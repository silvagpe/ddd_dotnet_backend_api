using DeveloperStore.Domain.Entities;
using SharpAbp.Abp.Snowflakes;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class CustomerTests
{
    private readonly Snowflake _snowflakeIdGenerator;

    public CustomerTests()
    {
        _snowflakeIdGenerator = new Snowflake(workerId: 1, datacenterId: 1);
    }
    
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var customer = new Customer(_snowflakeIdGenerator.NextId(), "FirstName", "LastName", "email@example.com", "123456789");

        Assert.Equal("FirstName", customer.FirstName);
        Assert.Equal("LastName", customer.LastName);
        Assert.Equal("email@example.com", customer.Email);
        Assert.Equal("123456789", customer.Phone);
        Assert.Equal("FirstName LastName", customer.FullName);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var customer = new Customer(_snowflakeIdGenerator.NextId(), "FirstName", "LastName", "email@example.com", "123456789");

        customer.UpdateDetails("NewFirstName", "NewLastName", "newemail@example.com", "987654321");

        Assert.Equal("NewFirstName", customer.FirstName);
        Assert.Equal("NewLastName", customer.LastName);
        Assert.Equal("newemail@example.com", customer.Email);
        Assert.Equal("987654321", customer.Phone);
        Assert.Equal("NewFirstName NewLastName", customer.FullName);
    }
}
