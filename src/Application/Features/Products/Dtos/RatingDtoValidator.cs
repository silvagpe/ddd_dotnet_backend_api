using FluentValidation;

namespace DeveloperStore.Application.Features.Products.Dtos;


public class RatingDtoValidator : AbstractValidator<RatingDto>
{
    public RatingDtoValidator()
    {
        RuleFor(command => command.Rate)
            .NotNull().WithMessage("Rate is required.")
            .InclusiveBetween(0, 5).WithMessage("Rate must be between 0 and 5.");
    }
}