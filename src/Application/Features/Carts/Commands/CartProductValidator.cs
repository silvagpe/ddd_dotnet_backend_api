using DeveloperStore.Application.Features.Carts.Commands;
using FluentValidation;

namespace DeveloperStore.Application.Features.Carts.Commands;


public class CartProductValidator : AbstractValidator<CartProduct>
{
    public CartProductValidator()
    {
        RuleFor(product => product.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(product => product.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}