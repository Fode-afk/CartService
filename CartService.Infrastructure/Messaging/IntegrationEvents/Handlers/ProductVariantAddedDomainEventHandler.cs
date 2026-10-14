using CatalogService.Domain.DomainEvents;
using CatalogService.Domain.Primitives;
using MassTransit;
using migApp.Shared.Dtos.Product;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.IntegrationEvents.Handlers;

public sealed class ProductVariantAddedDomainEventHandler(IPublishEndpoint publish) : IPreCommitDomainEventHandler<ProductVariantAddedDomainEvent>
{
    public async Task Handle(ProductVariantAddedDomainEvent notification, CancellationToken cancellationToken) =>
        await publish.Publish(new ProductVariantAddedIntegrationEvent(
            notification.ProductId,
            [.. notification.Attributes.Select(a => new ProductAttributeDto(
                a.CharacteristicId,
                a.Name,
                a.Value?.Value,
                a.CharType,
                a.GroupName?.Value,
                a.IsVariable,
                a.IsUnifying,
                [.. a.VariableValues.Select(v => new AttributeVariableValueDto(
                    v.Value,
                    v.ValueId))]))],
            notification.Version), cancellationToken);
}
