using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductCardTagsReplacedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductTagsReplacedDomainEvent>
{
    public async Task Handle(ProductTagsReplacedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductTagsReplacedIntegrationEvent(
            notification.ProductId,
            [.. notification.Tags.Select(t => t.Value)],
            notification.Version), cancellationToken);
}
