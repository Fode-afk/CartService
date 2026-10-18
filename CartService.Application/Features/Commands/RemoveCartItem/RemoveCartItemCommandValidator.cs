using CartService.Domain.Errors;
using FluentValidation;

namespace CartService.Application.Features.Commands.RemoveCartItem;

public sealed class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);

        RuleFor(x => x.VariantId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);
    }
}