using DeveloperStore.Domain.Entities;
using Xunit;

namespace DeveloperStore.UnitTests.Domain.Entities;

public class BranchTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var branch = new Branch("BranchName", "Address", "City", "State", "12345", "123456789");

        Assert.Equal("BranchName", branch.Name);
        Assert.Equal("Address", branch.Address);
        Assert.Equal("City", branch.City);
        Assert.Equal("State", branch.State);
        Assert.Equal("12345", branch.ZipCode);
        Assert.Equal("123456789", branch.Phone);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var branch = new Branch("BranchName", "Address", "City", "State", "12345", "123456789");

        branch.UpdateDetails("NewName", "NewAddress", "NewCity", "NewState", "54321", "987654321");

        Assert.Equal("NewName", branch.Name);
        Assert.Equal("NewAddress", branch.Address);
        Assert.Equal("NewCity", branch.City);
        Assert.Equal("NewState", branch.State);
        Assert.Equal("54321", branch.ZipCode);
        Assert.Equal("987654321", branch.Phone);
    }
}
