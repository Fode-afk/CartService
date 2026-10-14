using CatalogService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.AddProductVariantPriceSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Pricing;

namespace CartService.Infrastructure.Messaging.Consumers.Price;

public sealed class PriceCreatedIntegrationEventConsumer(IMediator mediator) : IConsumer<PriceCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<PriceCreatedIntegrationEvent> context) =>
        await mediator.Send(new AddProductVariantPriceSnapshotCommand(
            context.Message.ProductVariantId,
            context.Message.ProductId,
            context.Message.HasActivePrice,
            context.Message.Version), context.CancellationToken);
}
