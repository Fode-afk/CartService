using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Metrics;
using CartService.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.UpdateProductVariantPriceSnapshot;

public sealed class UpdateProductVariantPriceSnapshotCommandHandler(
    IAppDbContext context,
    ICartMetrics metrics,
    ILogger<UpdateProductVariantPriceSnapshotCommandHandler> logger,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantPriceSnapshotCommand>
{
    public async Task Handle(UpdateProductVariantPriceSnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductVariantPriceSnapshots
            .FirstOrDefaultAsync(p => p.PriceId == request.PriceId, cancellationToken);

        if (snapshot == null)
        {
            metrics.RecordSnapshotNotFound("ProductVariant");
            throw new SnapshotNotFoundException("ProductVariant", request.PriceId);
        }

        if (request.Version <= snapshot.Version)
        {
            metrics.RecordSnapshotOutdated(nameof(UpdateProductVariantPriceSnapshotCommand));
            return;
        }

        var currencyResult = Currency.Create(request.Currency);

        if (currencyResult.IsFailure)
        {
            logger.LogWarning(
               "Invalid currency in {Command}: ProductVariantId={ProductVariantId}, Version={Version}, Currency={Currency}, Error={@Error}",
               nameof(UpdateProductVariantPriceSnapshotCommand),
               request.PriceId,
               request.Version,
               request.Currency,
               currencyResult.Error);
            return;
        }

        snapshot.AmountMinor = request.UnitPriceMinor;
        snapshot.Currency = currencyResult.Value;
        snapshot.Version = request.Version;
        snapshot.UpdatedAt = timeProvider.GetUtcNow();

        await context.SaveChangesAsync(cancellationToken);
    }
}