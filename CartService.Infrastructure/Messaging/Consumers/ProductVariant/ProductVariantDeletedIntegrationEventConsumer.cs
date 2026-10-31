using CartService.Application.Features.IntegrationEventHandlers.ProductVariantSnapshot.DeleteProductVariantSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.ProductVariants;

namespace CartService.Infrastructure.Messaging.Consumers.ProductVariant;

public sealed class ProductVariantDeletedIntegrationEventConsumer(ISender sender) : IConsumer<ProductVariantDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductVariantDeletedIntegrationEvent> context) => 
        await sender.Send(new DeleteProductVariantSnapshotCommand(context.Message.ProductVariantId), context.CancellationToken);
}
