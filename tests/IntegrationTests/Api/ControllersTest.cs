using System.Net;
using System.Net.Http.Json;
using System.Security.Policy;
using DeveloperStore.Application.Features.Branches.Dtos;
using DeveloperStore.Application.Features.Carts.Commands;
using DeveloperStore.Application.Features.Carts.Dtos;
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
    public async Task Post_Product_Invalid_Should_Return_ValidationError()
    {
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
    public async Task Post_Product_Valid_Should_Return_ProductDto()
    {
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
        var response = await client.PostAsJsonAsync("/api/Products", productToPost);

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

    [Fact]
    public async Task Put_Product_Valid_Should_Return_ProductDto()
    {

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

        var response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var productPosted = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        productPosted.Should().NotBeNull();
        productPosted.Id.Should().BeGreaterThan(0);


        productPosted.Title = "Product 1 Updated";
        productPosted.Description = "Product 1 Description Updated";
        productPosted.Price = 200;
        productPosted.Rating = new RatingDto()
        {
            Rate = 2.2M,
            Count = 200
        };

        // Act        
        response = await client.PutAsJsonAsync($"/api/Products/{productPosted.Id}", productPosted);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var productUpdated = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        productUpdated.Should().NotBeNull();
        productUpdated.Id.Should().BeGreaterThan(0);
        productUpdated.Id.Should().Be(productPosted.Id);
        productUpdated.Title.Should().Contain("Updated");
        productUpdated.Description.Should().Contain("Updated");
        productUpdated.Price.Should().Be(200);
        productUpdated.Rating.Should().NotBeNull();
        productUpdated.Rating.Rate.Should().Be(2.2M);
        productUpdated.Rating.Count.Should().Be(200);
    }

    [Fact]
    public async Task Delete_Product_Valid_Should_Return_NotFound()
    {
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

        var response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var productPosted = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        productPosted.Should().NotBeNull();
        productPosted.Id.Should().BeGreaterThan(0);

        // Act        
        response = await client.DeleteAsync($"/api/Products/{productPosted.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await client.GetAsync($"/api/Products/{productPosted.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Product_Get_Categories_Should_Return_Array()
    {
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

        var response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act  
        response = await client.GetAsync("/api/Products/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var categories = System.Text.Json.JsonSerializer.Deserialize<List<string>>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        categories.Should().NotBeNull();
        categories.Count.Should().BeGreaterThan(0);
        categories.Should().Contain("category");

    }

    [Fact]
    public async Task Get_ProductsByCategory_FilterByTitle_And_OrderedDesc_Should_Return_Array()
    {
        // Arrange
        var client = _testFixture.Client;
        var productToPost = new CreateProductCommand
        {
            Title = "Product 1",
            Description = "Product 1 Description",
            Price = 100,
            Category = "men's clothing",
            Image = "image.png",
            Rating = new RatingDto
            {
                Rate = 1.1M,
                Count = 100
            },
        };

        var response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        productToPost = new CreateProductCommand
        {
            Title = "Product 2",
            Description = "Product 2 Description",
            Price = 100,
            Category = "men's clothing",
            Image = "image.png",
            Rating = new RatingDto
            {
                Rate = 1.1M,
                Count = 100
            },
        };

        response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        productToPost = new CreateProductCommand
        {
            Title = "xpto Product 3",
            Description = "xpto Product 3 Description",
            Price = 100,
            Category = "men's clothing",
            Image = "image.png",
            Rating = new RatingDto
            {
                Rate = 1.1M,
                Count = 100
            },
        };

        response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act  
        string parameters = Uri.EscapeDataString("men's clothing");
        response = await client.GetAsync($"/api/Products/category/{parameters}?title=pro*&_order=\"title\" desc");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var productsByCategory = System.Text.Json.JsonSerializer.Deserialize<PagedResult<ProductDto>>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        productsByCategory.Should().NotBeNull();
        productsByCategory.Data.Should().NotBeNull();
        productsByCategory.Data.Count().Should().BeGreaterThanOrEqualTo(2);
        productsByCategory.Data.Should().BeInDescendingOrder(b => b.Title);
    }

    [Fact]
    public async Task Get_ProductsByPrice_And_OrderedByPriceDesc_Should_Return_Array()
    {
        // Arrange
        var client = _testFixture.Client;
        var productToPost = new CreateProductCommand
        {
            Title = "Product 1",
            Description = "Product 1 Description",
            Price = 100,
            Category = "men's clothing",
            Image = "image.png",
            Rating = new RatingDto
            {
                Rate = 1.1M,
                Count = 100
            },
        };

        var response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        productToPost = new CreateProductCommand
        {
            Title = "Product 2",
            Description = "xpto Product 2 Description",
            Price = 200,
            Category = "men's clothing",
            Image = "image.png",
            Rating = new RatingDto
            {
                Rate = 1.1M,
                Count = 200
            },
        };

        response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        productToPost = new CreateProductCommand
        {
            Title = "Product 3",
            Description = "xpto Product 3 Description",
            Price = 300,
            Category = "men's clothing",
            Image = "image.png",
            Rating = new RatingDto
            {
                Rate = 1.1M,
                Count = 100
            },
        };

        response = await client.PostAsJsonAsync("/api/Products", productToPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act          
        response = await client.GetAsync($"/api/Products?_minPrice=199&_order=\"price desc\"");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var productsByCategory = System.Text.Json.JsonSerializer.Deserialize<PagedResult<ProductDto>>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        productsByCategory.Should().NotBeNull();
        productsByCategory.Data.Should().NotBeNull();
        productsByCategory.Data.Count().Should().BeGreaterThanOrEqualTo(2);
        productsByCategory.Data.Should().BeInDescendingOrder(b => b.Price);
    }

    [Fact]
    public async Task Post_Cart_Valid_Should_Return_CartDto()
    {
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

        var response = await client.PostAsJsonAsync("/api/Products", productToPost);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();

        var productResult = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


        var cartToPost = new CreateCartCommand
        {
            Date = DateTime.UtcNow,
            UserId = 1,
            Products = new List<CartProduct> {
                new CartProduct
                {
                    ProductId = productResult.Id,
                    Quantity = 1
                }
            }             
        };
        
        // Act
        response = await client.PostAsJsonAsync("/api/Carts", cartToPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        var cartResult = System.Text.Json.JsonSerializer.Deserialize<SaleDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        cartResult.Should().NotBeNull();
        cartResult.Id.Should().BeGreaterThan(0);
        cartResult.UserId.Should().Be(cartToPost.UserId);
        cartResult.Products.Should().NotBeNullOrEmpty();
        cartResult.Products.Count.Should().Be(cartToPost.Products.Count);
        cartResult.Products[0].ProductId.Should().Be(cartToPost.Products[0].ProductId);
    }

    [Fact]
    public async Task Put_Cart_Valid_Should_Return_CartDto()
    {
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

        var response = await client.PostAsJsonAsync("/api/Products", productToPost);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        var productResult = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


        var cartToPost = new CreateCartCommand
        {
            Date = DateTime.UtcNow,
            UserId = 1,
            Products = new List<CartProduct> {
                new CartProduct
                {
                    ProductId = productResult.Id,
                    Quantity = 1
                }
            }             
        };
        response = await client.PostAsJsonAsync("/api/Carts", cartToPost);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        var cartPosted = System.Text.Json.JsonSerializer.Deserialize<SaleDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        var cartToUpdate = new UpdateCartCommand
        {
            Date = DateTime.UtcNow,
            UserId = 1,
            Products = new List<CartProduct> {
                new CartProduct
                {
                    ProductId = productResult.Id,
                    Quantity = 4
                }
            }             
        };
        
        // Act
        response = await client.PutAsJsonAsync($"/api/Carts/{cartPosted.Id}", cartToUpdate);
        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        var cartResult = System.Text.Json.JsonSerializer.Deserialize<SaleDto>(
            responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        cartResult.Should().NotBeNull();
        cartResult.Id.Should().BeGreaterThan(0);
        cartResult.Id.Should().Be(cartPosted.Id);
        cartResult.UserId.Should().Be(cartToPost.UserId);
        cartResult.Products.Should().NotBeNullOrEmpty();
        cartResult.Products.Count.Should().Be(cartToPost.Products.Count);
        cartResult.Products[0].ProductId.Should().Be(cartToPost.Products[0].ProductId);
        cartResult.Products[0].Quantity.Should().Be(cartToUpdate.Products[0].Quantity);
    }

}