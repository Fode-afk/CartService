using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemAddedDomainEvent(Guid BuyerId) : IDomainEvent;