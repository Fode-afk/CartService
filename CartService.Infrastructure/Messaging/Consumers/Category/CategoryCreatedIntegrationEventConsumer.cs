using CatalogService.Application.Features.IntegrationEventHandlers.CategorySnapshot.AddCategorySnapshot;
using MassTransit;
using MediatR;
using migApp.Shared.Messaging.IntegrationEvents.Categories;

namespace CartService.Infrastructure.Messaging.Consumers.Category;

public sealed class CategoryCreatedIntegrationEventConsumer(IMediator mediator) : IConsumer<CategoryCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CategoryCreatedIntegrationEvent> context) =>
        await mediator.Send(new AddCategorySnapshotCommand(
            context.Message.CategoryId,
            context.Message.IsActive,
            context.Message.Version), context.CancellationToken);
}
