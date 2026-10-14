using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.Cart.CreateCart;

public sealed record CreateCartCommand(Guid BuyerId) : IRequest;