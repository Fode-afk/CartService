using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartClearedDomainEvent(
    Guid CartId,
    long Version) : IDomainEvent;