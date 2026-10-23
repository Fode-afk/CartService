using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemQuantityChangedDomainEvent(Guid BuyerId) : IDomainEvent;