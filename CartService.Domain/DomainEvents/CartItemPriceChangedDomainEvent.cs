using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemPriceChangedDomainEvent(
    Guid CartId,
    long Version) : IDomainEvent;