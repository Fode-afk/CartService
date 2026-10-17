using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductBlockedIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductBlockedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductBlockedIntegrationEvent> context) =>
        await mediator.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
