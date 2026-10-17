using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.DeleteProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductDeletedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedIntegrationEvent> context) =>
        await mediator.Send(new DeleteProductSnapshotCommand(context.Message.ProductId), context.CancellationToken);
}