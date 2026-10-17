using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemsSuspendedDomainEvent(Guid BuyerId) : IDomainEvent;