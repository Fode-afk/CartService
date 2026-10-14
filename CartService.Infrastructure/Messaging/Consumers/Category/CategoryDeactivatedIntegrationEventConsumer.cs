using CatalogService.Application.Features.IntegrationEventHandlers.CategorySnapshot.UpdateCategorySnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Categories;

namespace CartService.Infrastructure.Messaging.Consumers.Category;

public sealed class CategoryDeactivatedIntegrationEventConsumer(IMediator mediator) : IConsumer<CategoryDeactivatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CategoryDeactivatedIntegrationEvent> context) =>
        await mediator.Send(new UpdateCategorySnapshotCommand(
            context.Message.CategoryId,
            context.Message.IsActive,
            context.Message.Version), context.CancellationToken);
}
