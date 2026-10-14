using CatalogService.Application.Features.IntegrationEventHandlers.VendorSnapshot.AddVendorSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Vendors;

namespace CartService.Infrastructure.Messaging.Consumers.Vendor;

public sealed class VendorCreatedIntegrationEventConsumer(IMediator mediator) : IConsumer<VendorCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<VendorCreatedIntegrationEvent> context) =>
        await mediator.Send(new AddVendorSnapshotCommand(
            context.Message.VendorId, 
            context.Message.IsActive,
            context.Message.Version), context.CancellationToken);
}