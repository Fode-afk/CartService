using CartService.Domain.Errors;
using FluentValidation;

namespace CartService.Application.Features.Queries.GetCart;

public sealed class GetCartQueryValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x.BuyerId)
            .NotEmpty().WithErrorCode(CartErrorCodes.InvalidId);
    }
}