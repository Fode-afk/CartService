using CartService.Domain.Errors;
using FluentValidation;

namespace CartService.Application.Features.Commands.AddCartItem;

public sealed class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
{
    public AddCartItemCommandValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);

        RuleFor(x => x.VariantId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithErrorCode(CartItemErrorCodes.InvalidQuantity);
    }
}