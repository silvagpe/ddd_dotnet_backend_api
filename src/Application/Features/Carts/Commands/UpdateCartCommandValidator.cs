using DeveloperStore.Application.Features.Carts.Commands;
using FluentValidation;

namespace DeveloperStore.Application.Features.Carts.Commands;


public class UpdateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public UpdateCartCommandValidator()
    {
        RuleFor(command => command.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0.");

        RuleFor(command => command.Date)
            .NotEmpty().WithMessage("Date is required.")
            .Must(date => date != default(DateTime)).WithMessage("Invalid date format.");

        RuleFor(command => command.Products)
            .NotEmpty().WithMessage("At least one product is required.")
            .Must(products => products.Count > 0).WithMessage("At least one product is required.")
            .ForEach(product =>
            {
                product.SetValidator(new CartProductValidator());
            });
            
    }
}