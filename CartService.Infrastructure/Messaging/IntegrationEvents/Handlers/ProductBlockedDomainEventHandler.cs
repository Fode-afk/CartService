using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductBlockedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductBlockedDomainEvent>
{
    public async Task Handle(ProductBlockedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductBlockedIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.VendorId,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.IsBlocked,
            notification.ProductStatus,
            notification.BlockReason?.ToString(),
            notification.IsVisiblePublicly,
            notification.Version), cancellationToken);
}
