using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductDeletedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductDeletedDomainEvent>
{
    public async Task Handle(ProductDeletedDomainEvent notification, CancellationToken cancellationToken) => 
        await publish.Publish(new ProductDeletedIntegrationEvent(notification.ProductId), cancellationToken);
}
