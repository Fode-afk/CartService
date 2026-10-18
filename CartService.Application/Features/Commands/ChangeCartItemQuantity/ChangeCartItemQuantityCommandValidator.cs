using CartService.Domain.Errors;
using FluentValidation;

namespace CartService.Application.Features.Commands.ChangeCartItemQuantity;

public sealed class ChangeCartItemQuantityCommandValidator : AbstractValidator<ChangeCartItemQuantityCommand>
{
    public ChangeCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);

        RuleFor(x => x.VariantId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithErrorCode(CartItemErrorCodes.InvalidQuantity);
    }
}