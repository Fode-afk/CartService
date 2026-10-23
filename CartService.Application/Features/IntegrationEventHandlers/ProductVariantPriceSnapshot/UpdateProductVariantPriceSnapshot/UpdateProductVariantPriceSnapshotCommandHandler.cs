using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Jobs;
using CartService.Application.Interfaces.Metrics;
using CartService.Domain.Exceptions;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.UpdateProductVariantPriceSnapshot;

public sealed class UpdateProductVariantPriceSnapshotCommandHandler(
    IAppDbContext context,
    IBackgroundJobClient backgroundJobClient,
    ICartMetrics metrics,
    ILogger<UpdateProductVariantPriceSnapshotCommandHandler> logger,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantPriceSnapshotCommand>
{
    public async Task Handle(UpdateProductVariantPriceSnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductVariantPriceSnapshots
            .FirstOrDefaultAsync(p => p.ProductVariantId == request.ProductVariantId, cancellationToken);

        if (snapshot == null)
        {
            metrics.RecordSnapshotNotFound("ProductVariant");
            throw new SnapshotNotFoundException("ProductVariant", request.ProductVariantId);
        }

        if (request.Version <= snapshot.Version)
        {
            metrics.RecordSnapshotOutdated(nameof(UpdateProductVariantPriceSnapshotCommand));
            return;
        }

        var currencyResult = Currency.Create(request.Currency);

        if (currencyResult.IsFailure)
        {
            // log
            return;
        }

        var unitPriceResult = Money.FromMinor(request.UnitPriceMinor, currencyResult.Value);

        if (unitPriceResult.IsFailure)
        {
            logger.LogWarning(
                "Invalid price in {Command}: ProductVariantId={ProductVariantId}, Version={Version}, UnitPriceInUsdMinor={UnitPriceInUsdMinor}, Error={@Error}",
                nameof(UpdateProductVariantPriceSnapshotCommand),
                request.ProductVariantId,
                request.Version,
                request.UnitPriceMinor,
                unitPriceResult.Error);
            return;
        }

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        snapshot.UnitPrice = unitPriceResult.Value;
        snapshot.Version = request.Version;
        snapshot.UpdatedAt = timeProvider.GetUtcNow();

        await context.SaveChangesAsync(cancellationToken);

        backgroundJobClient.Enqueue<ISyncCartItemPricesJob>(
           job => job.Execute(request.ProductVariantId, unitPriceResult.Value, CancellationToken.None));

        await transaction.CommitAsync(cancellationToken);
    }
}
