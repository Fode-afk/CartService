using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.AddProductVariantPriceSnapshot;

public sealed record AddProductVariantPriceSnapshotCommand(
    Guid ProductVariantId, 
    long UnitPriceMinor,
    string Currency,
    long Version) : IRequest;