using FluentValidation;

namespace DeveloperStore.Application.Features.Products.Commands;


public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}