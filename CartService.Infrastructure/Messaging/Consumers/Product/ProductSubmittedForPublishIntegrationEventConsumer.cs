using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductSubmittedForPublishIntegrationEventConsumer(IMediator mediator) : IConsumer<ProductSubmittedForPublishIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductSubmittedForPublishIntegrationEvent> context) =>
        await mediator.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
