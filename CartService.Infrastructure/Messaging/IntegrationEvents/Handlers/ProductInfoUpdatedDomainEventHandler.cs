using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductInfoUpdatedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductInfoUpdatedDomainEvent>
{
    public async Task Handle(ProductInfoUpdatedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductInfoUpdatedIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.BrandId,
            notification.Name,
            notification.Slug,
            notification.Description,
            notification.ShortDescription,
            notification.SeoMetadata.Title,
            notification.SeoMetadata.Description,
            notification.SeoMetadata.Keywords,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.Version), cancellationToken);
}
