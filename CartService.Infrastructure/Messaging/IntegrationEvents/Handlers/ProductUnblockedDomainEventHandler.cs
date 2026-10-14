using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductUnblockedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductUnblockedDomainEvent>
{
    public async Task Handle(ProductUnblockedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductUnblockedIntegrationEvent(
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
