using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductPublishSubmissionWithdrawnDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductPublishSubmissionWithdrawnDomainEvent>
{
    public async Task Handle(ProductPublishSubmissionWithdrawnDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductPublishSubmissionWithdrawnIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.VendorId,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.ProductStatus,
            notification.IsVisiblePublicly,
            notification.Version), cancellationToken);
}