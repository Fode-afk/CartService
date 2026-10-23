using CartService.Domain.Errors;
using FluentValidation;
using migApp.Shared.Domain.Errors;

namespace CartService.Application.Features.Queries.GetCart;

public sealed class GetCartQueryValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3)
            .WithErrorCode(CurrencyErrorCodes.InvalidFormat);
    }
}