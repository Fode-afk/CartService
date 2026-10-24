using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.AddProductVariantPriceSnapshot;

public sealed record AddProductVariantPriceSnapshotCommand(
    Guid PriceId,
    Guid ProductVariantId, 
    bool IsBase,
    long UnitPriceMinor,
    string Currency,
    long Version) : IRequest;