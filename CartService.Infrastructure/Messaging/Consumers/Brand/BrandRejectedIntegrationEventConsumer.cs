using CatalogService.Application.Features.IntegrationEventHandlers.BrandSnapshot.UpdateBrandSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Brands;

namespace CartService.Infrastructure.Messaging.Consumers.Brand;

public sealed class BrandRejectedIntegrationEventConsumer(IMediator mediator) : IConsumer<BrandRejectedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BrandRejectedIntegrationEvent> context) =>
        await mediator.Send(new UpdateBrandSnapshotCommand(
            context.Message.BrandId,
            context.Message.IsAssignable,
            context.Message.Version), context.CancellationToken);
}