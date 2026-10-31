using CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Products;

namespace CartService.Infrastructure.Messaging.Consumers.Product;

public sealed class ProductPublishSubmissionWithdrawnIntegrationEventConsumer(ISender sender) : IConsumer<ProductPublishSubmissionWithdrawnIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPublishSubmissionWithdrawnIntegrationEvent> context) =>
        await sender.Send(new UpdateProductSnapshotCommand(
            context.Message.ProductId,
            context.Message.IsVisiblePublicly,
            context.Message.Version), context.CancellationToken);
}
