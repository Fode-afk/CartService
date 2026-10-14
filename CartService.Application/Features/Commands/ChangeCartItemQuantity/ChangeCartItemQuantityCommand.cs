using MediatR;
using migApp.Shared.Results;

namespace CartService.Application.Features.Commands.ChangeCartItemQuantity;

public sealed record ChangeCartItemQuantityCommand(
    Guid BuyerId,
    Guid VariantId,
    int Quantity) : IRequest<IResult>;