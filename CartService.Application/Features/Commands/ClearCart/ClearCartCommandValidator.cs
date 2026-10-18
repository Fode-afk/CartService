using CartService.Domain.Errors;
using FluentValidation;

namespace CartService.Application.Features.Commands.ClearCart;

public sealed class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);
    }
}