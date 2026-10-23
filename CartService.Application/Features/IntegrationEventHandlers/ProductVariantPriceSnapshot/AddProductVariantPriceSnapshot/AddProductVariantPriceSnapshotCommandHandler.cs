using CartService.Application.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.AddProductVariantPriceSnapshot;

public sealed class AddProductVariantPriceSnapshotCommandHandler(
    IAppDbContext context,
    ILogger<AddProductVariantPriceSnapshotCommandHandler> logger,
    TimeProvider timeProvider) : IRequestHandler<AddProductVariantPriceSnapshotCommand>
{
    public async Task Handle(AddProductVariantPriceSnapshotCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.ProductVariantPriceSnapshots
            .AnyAsync(x => x.ProductVariantId == request.ProductVariantId, cancellationToken);
        if (exists)
            return;

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
                nameof(AddProductVariantPriceSnapshotCommand),
                request.ProductVariantId,
                request.Version,
                request.UnitPriceMinor,
                unitPriceResult.Error);
            return;
        }
        
        context.ProductVariantPriceSnapshots.Add(
            new Domain.Snapshots.ProductVariantPriceSnapshot
            {
                ProductVariantId = request.ProductVariantId,
                UnitPrice = unitPriceResult.Value,
                UpdatedAt = timeProvider.GetUtcNow(),
                Version = request.Version
            });

        await context.SaveChangesAsync(cancellationToken);
    }
}