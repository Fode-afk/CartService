using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.UpdateProductVariantPriceSnapshot;

public sealed record UpdateProductVariantPriceSnapshotCommand(
    Guid ProductVariantId,
    long UnitPriceMinor,
    string Currency,
    long Version) : IRequest;