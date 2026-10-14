using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.AddProductVariantPriceSnapshot;

public sealed record AddProductVariantPriceSnapshotCommand(
    Guid ProductVariantId, 
    Guid ProductId,
    long UnitPriceInUsdMinor,
    long Version) : IRequest;