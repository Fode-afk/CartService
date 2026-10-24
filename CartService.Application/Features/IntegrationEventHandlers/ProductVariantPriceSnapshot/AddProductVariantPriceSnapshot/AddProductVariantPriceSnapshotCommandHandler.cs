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
            .AnyAsync(x => x.PriceId == request.PriceId, cancellationToken);
        if (exists)
            return;

        var currencyResult = Currency.Create(request.Currency);

        if (currencyResult.IsFailure)
        {
            logger.LogWarning(
                "Invalid currency in {Command}: ProductVariantId={ProductVariantId}, Version={Version}, Currency={Currency}, Error={@Error}",
                nameof(AddProductVariantPriceSnapshotCommand),
                request.ProductVariantId,
                request.Version,
                request.Currency,
                currencyResult.Error);
            return;
        }
        
        context.ProductVariantPriceSnapshots.Add(
            new Domain.Snapshots.ProductVariantPriceSnapshot
            {
                PriceId = request.PriceId,
                ProductVariantId = request.ProductVariantId,
                IsBase = request.IsBase,
                AmountMinor = request.UnitPriceMinor,
                Currency = currencyResult.Value,
                UpdatedAt = timeProvider.GetUtcNow(),
                Version = request.Version
            });

        await context.SaveChangesAsync(cancellationToken);
    }
}