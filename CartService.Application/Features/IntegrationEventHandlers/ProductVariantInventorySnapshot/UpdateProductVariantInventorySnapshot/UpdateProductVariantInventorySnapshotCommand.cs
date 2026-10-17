using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantInventorySnapshot.UpdateProductVariantInventorySnapshot;

public sealed record UpdateProductVariantInventorySnapshotCommand(
    Guid ProductVariantId,
    int AvailableQuantity,
    long Version) : IRequest;