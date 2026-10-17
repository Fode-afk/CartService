using CartService.Domain.Primitives;

namespace CartService.Infrastructure.DomainEvents;

internal interface IDomainEventsDispatcher
{
    Task DispatchPreCommitDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
    Task DispatchPostCommitDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}