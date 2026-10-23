using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartItemPriceChangedDomainEvent(Guid BuyerId) : IDomainEvent;