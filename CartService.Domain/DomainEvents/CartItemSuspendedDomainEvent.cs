using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemSuspendedDomainEvent(Guid BuyerId) : IDomainEvent;