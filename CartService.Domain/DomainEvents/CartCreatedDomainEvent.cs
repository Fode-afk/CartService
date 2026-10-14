using CartService.Domain.Primitives;

namespace CartService.Domain.DomainEvents;

public sealed record CartCreatedDomainEvent(Guid CartId) : IDomainEvent;