using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductUnpublishedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductUnpublishedDomainEvent>
{
    public async Task Handle(ProductUnpublishedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductUnpublishedIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.VendorId,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.ProductStatus,
            notification.IsVisiblePublicly,
            notification.Version), cancellationToken);
}
