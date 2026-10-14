using CatalogService.Application.Features.IntegrationEventHandlers.BrandSnapshot.AddBrandSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Brands;

namespace CartService.Infrastructure.Messaging.Consumers.Brand;

public sealed class BrandCreatedIntegrationEventConsumer(IMediator mediator) : IConsumer<BrandCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BrandCreatedIntegrationEvent> context) =>
        await mediator.Send(new AddBrandSnapshotCommand(
            context.Message.BrandId,
            context.Message.IsAssignable,
            context.Message.Version), context.CancellationToken);
}
