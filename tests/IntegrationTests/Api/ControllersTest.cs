using System.Net;
using DeveloperStore.Application.Features.Branches.Dtos;
using DeveloperStore.Application.Models;
using FluentAssertions;

namespace IntegrationTests.Api;

public class ControllersTest : IClassFixture<TestFixture>
{
    private readonly TestFixture _testFixture;

    public ControllersTest(TestFixture testFixture)
    {
        _testFixture = testFixture;
    }

    [Fact]
    public void ConnectionStringTest()
    {
        // Exemplo de uso do _testFixture
        var connectionString = _testFixture.ConnectionString;

        connectionString.Should().NotBeNullOrEmpty();
        connectionString.Should().Contain("user");

        // Adicione aqui o código do teste
    }

    [Fact]
    public async Task Get_BranchesById_Should_Return_Success()
    {
        // Arrange
        var client = _testFixture.Client;

        // Act
        var response = await client.GetAsync("/api/Branches/1");        

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        responseContent.Should().Contain("Main Branch");
    }

    [Fact]
    public async Task Get_BranchesById_Return_NotFound()
    {
        // Arrange
        var client = _testFixture.Client;

        // Act
        var response = await client.GetAsync("/api/Branches/99");        

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_BranchesByOrder_Should_Return_Success()
    {
        // Arrange
        var client = _testFixture.Client;

        // Act
        var response = await client.GetAsync("/api/Branches?_order=name desc, address asc");        

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        responseContent.Should().Contain("Main Branch");

        // Parse the response content to validate the order
        var branches = System.Text.Json.JsonSerializer.Deserialize<PagedResult<BranchDto>>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        branches.Should().NotBeNull();
        var items = branches.Data;
        items.Should().NotBeNullOrEmpty();
        items.Should().BeInDescendingOrder(b => b.Name);
    }
}