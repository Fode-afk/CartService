using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemRestoredDomainEvent(Guid BuyerId) : IDomainEvent;
