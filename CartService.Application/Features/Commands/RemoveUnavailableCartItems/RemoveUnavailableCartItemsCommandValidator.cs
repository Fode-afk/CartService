using CartService.Domain.Errors;
using FluentValidation;

namespace CartService.Application.Features.Commands.RemoveUnavailableCartItems;

public sealed class RemoveUnavailableCartItemsCommandValidator : AbstractValidator<RemoveUnavailableCartItemsCommand>
{
    public RemoveUnavailableCartItemsCommandValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);
    }
}