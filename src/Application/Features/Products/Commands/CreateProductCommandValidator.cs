using DeveloperStore.Application.Features.Products.Dtos;
using FluentValidation;

namespace DeveloperStore.Application.Features.Products.Commands;


public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(command => command.Description)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(command => command.Category)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(command => command.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(command => command.Rating)
            .NotNull().WithMessage("Rating is required.")
            .SetValidator(new RatingDtoValidator());
            
    }
}