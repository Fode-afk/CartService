using CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.UpdateProductVariantPriceSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Pricing;

namespace CartService.Infrastructure.Messaging.Consumers.Price;

public sealed class PriceUpdatedIntegrationEventConsumer(IMediator mediator) : IConsumer<PriceUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<PriceUpdatedIntegrationEvent> context) =>
        await mediator.Send(new UpdateProductVariantPriceSnapshotCommand(
            context.Message.ProductVariantId,
            context.Message.CurrentPriceMinor,
            context.Message.Currency,
            context.Message.Version), context.CancellationToken);
}