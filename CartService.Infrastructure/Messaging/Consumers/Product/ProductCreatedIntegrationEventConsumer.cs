using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.AddProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductCreatedIntegrationEventConsumer(ISender sender) : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context) =>
        await sender.Send(new AddProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.VendorId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
