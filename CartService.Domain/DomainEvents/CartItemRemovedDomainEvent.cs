using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemRemovedDomainEvent(Guid BuyerId) : IDomainEvent;