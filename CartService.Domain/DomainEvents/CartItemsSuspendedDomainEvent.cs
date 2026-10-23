using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemsSuspendedDomainEvent(Guid BuyerId) : IDomainEvent;