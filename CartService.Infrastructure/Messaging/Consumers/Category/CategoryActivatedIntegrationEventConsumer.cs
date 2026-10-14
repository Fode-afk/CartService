using CatalogService.Application.Features.IntegrationEventHandlers.CategorySnapshot.UpdateCategorySnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Categories;

namespace CartService.Infrastructure.Messaging.Consumers.Category;

public sealed class CategoryActivatedIntegrationEventConsumer(IMediator mediator) : IConsumer<CategoryActivatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CategoryActivatedIntegrationEvent> context) =>
        await mediator.Send(new UpdateCategorySnapshotCommand(
            context.Message.CategoryId,
            context.Message.IsActive,
            context.Message.Version), context.CancellationToken);
}
