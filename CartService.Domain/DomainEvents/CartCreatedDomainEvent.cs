using migApp.Shared.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartCreatedDomainEvent(Guid BuyerId) : IDomainEvent;