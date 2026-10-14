using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;
namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductCreatedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductCreatedDomainEvent>
{
    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(
            new ProductCreatedIntegrationEvent(
                notification.ProductId,
                notification.CategoryId,
                notification.VendorId,
                notification.BrandId,
                notification.Name,
                notification.Slug,
                notification.Description,
                notification.ShortDescription,
                notification.SeoMetadata.Title,
                notification.SeoMetadata.Description,
                notification.SeoMetadata.Keywords,
                notification.ProductStatus,
                notification.CanEditContent,
                notification.CanEditOperationalData,
                notification.IsVisiblePublicly,
                notification.Version), cancellationToken);
}