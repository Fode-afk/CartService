using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantSnapshot.AddProductVariantSnapshot;

public sealed record AddProductVariantSnapshotCommand(
    Guid ProductVariantId,
    Guid ProductId) : IRequest;