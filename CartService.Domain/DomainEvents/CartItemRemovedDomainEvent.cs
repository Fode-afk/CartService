using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemRemovedDomainEvent(Guid BuyerId) : IDomainEvent;