using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemQuantityChangedDomainEvent(
    Guid CartId,
    long Version) : IDomainEvent;