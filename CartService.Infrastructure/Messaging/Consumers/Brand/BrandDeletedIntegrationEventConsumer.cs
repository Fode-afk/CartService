using CatalogService.Application.Features.IntegrationEventHandlers.BrandSnapshot.DeleteBrandSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Brands;

namespace CartService.Infrastructure.Messaging.Consumers.Brand;

public sealed class BrandDeletedIntegrationEventConsumer(IMediator mediator) : IConsumer<BrandDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BrandDeletedIntegrationEvent> context) =>
        await mediator.Send(new DeleteBrandSnapshotCommand(context.Message.BrandId), context.CancellationToken);
}
