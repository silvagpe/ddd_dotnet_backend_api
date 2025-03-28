using DeveloperStore.Application.Features.Products.Dtos;
using MediatR;

namespace DeveloperStore.Application.Features.Products.Commands;

public class CreateProductCommand: IRequest<ProductDto?>
{
    public long Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Image { get; set; }
    public RatingDto Rating { get; set; } 
}