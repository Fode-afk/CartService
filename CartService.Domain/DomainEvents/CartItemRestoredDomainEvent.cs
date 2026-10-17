using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemRestoredDomainEvent(Guid BuyerId) : IDomainEvent;
