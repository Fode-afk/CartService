using Microsoft.Extensions.DependencyInjection;
using migApp.Shared.Domain.Primitives;

namespace CartService.Infrastructure.DomainEvents;

internal sealed class DomainEventsDispatcher(IServiceProvider serviceProvider) : IDomainEventsDispatcher
{
    public async Task DispatchPreCommitDomainEventsAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IPreCommitDomainEventHandler<>)
                .MakeGenericType(domainEvent.GetType());

            var handlers = serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
                await ((dynamic)handler!).Handle((dynamic)domainEvent, cancellationToken);
        }
    }

    public async Task DispatchPostCommitDomainEventsAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IPostCommitDomainEventHandler<>)
                .MakeGenericType(domainEvent.GetType());

            var handlers = serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
                await ((dynamic)handler!).Handle((dynamic)domainEvent, cancellationToken);
        }
    }
}
