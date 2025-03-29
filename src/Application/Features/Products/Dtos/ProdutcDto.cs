namespace DeveloperStore.Application.Features.Products.Dtos;

public class ProductDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;
    public RatingDto Rating { get; set; } = new RatingDto();
}