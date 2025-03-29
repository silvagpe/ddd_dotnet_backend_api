using FluentValidation;

namespace DeveloperStore.Application.Features.Products.Queries;


public class GetProductsByCategoryQueryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    public GetProductsByCategoryQueryValidator()
    {
        RuleFor(command => command.Category)
            .NotNull().WithMessage("Category is required.");
    }
}