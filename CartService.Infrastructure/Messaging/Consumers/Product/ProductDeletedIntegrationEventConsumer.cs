using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.DeleteProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductDeletedIntegrationEventConsumer(ISender sender) : IConsumer<ProductDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedIntegrationEvent> context) =>
        await sender.Send(new DeleteProductSnapshotCommand(context.Message.ProductId), context.CancellationToken);
}