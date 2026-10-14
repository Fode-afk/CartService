using MediatR;
using migApp.Shared.Results;

namespace CartService.Application.Features.Commands.RemoveCartItem;

public sealed record RemoveCartItemCommand(
    Guid BuyerId,
    Guid VariantId) : IRequest<IResult>;