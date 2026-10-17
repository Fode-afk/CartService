using CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.DeleteProductVariantPriceSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Pricing;

namespace CartService.Infrastructure.Messaging.Consumers.Price;

public sealed class PriceArchivedIntegrationEventConsumer(IMediator mediator) : IConsumer<PriceArchivedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<PriceArchivedIntegrationEvent> context) =>
        await mediator.Send(new DeleteProductVariantPriceSnapshotCommand(context.Message.ProductVariantId), context.CancellationToken);
}
