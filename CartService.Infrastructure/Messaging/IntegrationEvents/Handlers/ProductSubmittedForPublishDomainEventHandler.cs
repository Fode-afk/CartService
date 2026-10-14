using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductSubmittedForPublishDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductSubmittedForPublishDomainEvent>
{
    public async Task Handle(ProductSubmittedForPublishDomainEvent notification, CancellationToken cancellationToken) => 
        await publish.Publish(new ProductSubmittedForPublishIntegrationEvent(
            notification.ProductId,
            notification.CategoryId,
            notification.VendorId,
            notification.CanEditContent,
            notification.CanEditOperationalData,
            notification.ProductStatus,
            [..notification.SuspensionReasons.Select(r => r.Reason)],
            notification.RejectionReason?.ToString(),
            notification.SubmittedForPublishApprovalAt,
            notification.IsVisiblePublicly,
            notification.Version), cancellationToken);
}
