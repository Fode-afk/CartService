using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemAddedDomainEvent(
    Guid CartId,
    long Version) : IDomainEvent;