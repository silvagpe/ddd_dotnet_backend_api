using System.Net;
using System.Net.Http.Json;
using DeveloperStore.Application.Features.Branches.Dtos;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
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

    [Fact]
    public async Task Post_Product_Invalid_Should_Return_ValidationError(){
        // Arrange
        var client = _testFixture.Client;
        var product = new CreateProductCommand
        {
            Id = 0,
            Title = "",
            Description = "",
            Price = 0,
            Category = "",    
            Rating = new RatingDto
            {
                Rate = 0,
                Count = 0
            },        
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Products", product);        

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        responseContent.Should().Contain("Title is required.");
        responseContent.Should().Contain("Description is required.");
        responseContent.Should().Contain("Category is required.");
        responseContent.Should().Contain("Price must be greater than 0.");
    }
    
    [Fact]
    public async Task Post_Product_Valid_Should_Return_ProductDto(){
        // Arrange
        var client = _testFixture.Client;
        var productToPost = new CreateProductCommand
        {            
            Title = "Product 1",
            Description = "Product 1 Description",
            Price = 100,
            Category = "category",
            Image = "image.png",    
            Rating = new RatingDto
            {
                Rate = 1.1M,
                Count = 100
            },        
        };

        // Act
        var response = await client.PostAsJsonAsync<CreateProductCommand>("/api/Products", productToPost);        

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        
        var productResult = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        
        productResult.Should().NotBeNull();
        productResult.Id.Should().BeGreaterThan(0);
        productResult.Title.Should().Be(productToPost.Title);   
        productResult.Description.Should().Be(productToPost.Description);
        productResult.Price.Should().Be(productToPost.Price);
        productResult.Category.Should().Be(productToPost.Category);
        productResult.Image.Should().Be(productToPost.Image);
        productResult.Rating.Should().NotBeNull();
        productResult.Rating.Rate.Should().Be(productToPost.Rating.Rate);
        productResult.Rating.Count.Should().Be(productToPost.Rating.Count);
        
    }

}