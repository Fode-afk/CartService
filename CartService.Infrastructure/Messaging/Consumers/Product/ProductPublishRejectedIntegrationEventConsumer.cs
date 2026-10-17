using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductPublishRejectedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductPublishRejectedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPublishRejectedIntegrationEvent> context) =>
        await mediator.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
