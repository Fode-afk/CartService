using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemsRemovedUnavailableDomainEvent(Guid BuyerId) : IDomainEvent;