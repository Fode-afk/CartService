using MediatR;
using migApp.Shared.Results;

namespace CartService.Application.Features.Commands.AddCartItem;

public sealed record AddCartItemCommand(
    Guid BuyerId,
    Guid VariantId,
    int Quantity) : IRequest<IResult>;