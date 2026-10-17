using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemSuspendedDomainEvent(Guid BuyerId) : IDomainEvent;