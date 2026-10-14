using CatalogService.Application.Features.IntegrationEventHandlers.CategorySnapshot.DeleteCategorySnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Categories;

namespace CartService.Infrastructure.Messaging.Consumers.Category;

public sealed class CategoryDeletedIntegrationEventConsumer(IMediator mediator) : IConsumer<CategoryDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CategoryDeletedIntegrationEvent> context) =>
        await mediator.Send(new DeleteCategorySnapshotCommand(context.Message.CategoryId), context.CancellationToken);
}
