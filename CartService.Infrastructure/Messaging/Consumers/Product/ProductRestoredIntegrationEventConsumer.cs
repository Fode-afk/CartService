using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductRestoredIntegrationEventConsumer(ISender sender) : IConsumer<ProductRestoredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductRestoredIntegrationEvent> context) =>
        await sender.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
