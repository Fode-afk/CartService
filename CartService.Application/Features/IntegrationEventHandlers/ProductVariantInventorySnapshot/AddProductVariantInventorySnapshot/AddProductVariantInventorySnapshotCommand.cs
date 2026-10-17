using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantInventorySnapshot.AddProductVariantInventorySnapshot;

public sealed record AddProductVariantInventorySnapshotCommand(
    Guid ProductVariantId,
    int AvailableQuantity,
    long Version) : IRequest;