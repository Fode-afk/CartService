using MediatR;

namespace CartService.Domain.Primitives;

public interface IPostCommitDomainEventHandler<in T> : INotificationHandler<T>
    where T : IDomainEvent;
