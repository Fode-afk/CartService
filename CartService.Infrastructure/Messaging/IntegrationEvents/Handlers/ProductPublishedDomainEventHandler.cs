using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductPublishedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductPublishedDomainEvent>
{
    public async Task Handle(ProductPublishedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductPublishedIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.VendorId,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.ProductStatus,
            notification.ApprovedAt,
            notification.IsVisiblePublicly,
            notification.Version), cancellationToken);
}