using CatalogService.Application.Features.IntegrationEventHandlers.BrandSnapshot.UpdateBrandSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Brands;

namespace CartService.Infrastructure.Messaging.Consumers.Brand;

public sealed class BrandDeactivatedIntegrationEventConsumer(IMediator mediator) : IConsumer<BrandDeactivatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BrandDeactivatedIntegrationEvent> context) =>
        await mediator.Send(new UpdateBrandSnapshotCommand(
            context.Message.BrandId,
            context.Message.IsAssignable,
            context.Message.Version), context.CancellationToken);
}
