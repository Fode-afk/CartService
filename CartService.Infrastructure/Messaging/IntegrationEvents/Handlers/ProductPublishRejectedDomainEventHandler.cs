using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductPublishRejectedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductPublishRejectedDomainEvent>
{
    public async Task Handle(ProductPublishRejectedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductPublishRejectedIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.VendorId,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.ProductStatus,
            notification.RejectionReason?.ToString(),
            notification.IsVisiblePublicly,
            notification.Version), cancellationToken);
}