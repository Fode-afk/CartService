using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.UpdateProductVariantPriceSnapshot;

public sealed record UpdateProductVariantPriceSnapshotCommand(
    Guid ProductVariantId,
    long UnitPriceInUsdMinor,
    long Version) : IRequest;