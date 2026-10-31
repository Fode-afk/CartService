using CartService.Application.Features.IntegrationEventHandlers.ProductVariantSnapshot.AddProductVariantSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.ProductVariants;

namespace CartService.Infrastructure.Messaging.Consumers.ProductVariant;

public sealed class ProductVariantCreatedIntegrationEventConsumer(ISender sender) : IConsumer<ProductVariantCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductVariantCreatedIntegrationEvent> context) =>
        await sender.Send(new AddProductVariantSnapshotCommand(
            context.Message.ProductVariantId,
            context.Message.ProductId), context.CancellationToken);
}