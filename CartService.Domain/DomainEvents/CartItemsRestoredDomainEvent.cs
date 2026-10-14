using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemsRestoredDomainEvent(
    Guid CartId,
    long Version) : IDomainEvent;