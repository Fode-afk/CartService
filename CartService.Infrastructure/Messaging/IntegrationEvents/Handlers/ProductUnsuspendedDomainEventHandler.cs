using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductUnsuspendedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductUnsuspendedDomainEvent>
{
    public async Task Handle(ProductUnsuspendedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductUnsuspendedIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.VendorId,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.ProductStatus,
            [..notification.SuspensionReasons.Select(r => r.Reason)],
            notification.IsVisiblePublicly,
            notification.Version), cancellationToken);
}
