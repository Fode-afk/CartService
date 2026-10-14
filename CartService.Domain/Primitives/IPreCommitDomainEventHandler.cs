using MediatR;

namespace CartService.Domain.Primitives;

public interface IPreCommitDomainEventHandler<in T> : INotificationHandler<T>
    where T : IDomainEvent;
