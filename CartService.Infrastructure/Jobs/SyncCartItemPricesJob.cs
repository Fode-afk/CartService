using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Jobs;
using CartService.Application.Interfaces.Metrics;
using CartService.Application.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Infrastructure.Jobs;

internal sealed class SyncCartItemPricesJob(
    IAppDbContext context,
    ICartMetrics metrics,
    ILogger<SuspendProductCartItemsJob> logger,
    TimeProvider timeProvider) : ISyncCartItemPricesJob
{
    public async Task Execute(Guid variantId, Money unitPrice, CancellationToken cancellationToken = default)
    {
        const int batchSize = 100;
        var offset = 0;

        while (true)
        {
            var carts = await context.Carts
                .Include(c => c.Items)
                .Where(c => c.Items.Any(i => i.VariantId == variantId))
                .OrderBy(p => p.Id)
                .Skip(offset)
                .Take(batchSize)
                .ToListAsync(cancellationToken);

            if (carts.Count == 0)
                break;

            foreach (var cart in carts)
            {
                var result = cart.SyncItemPrice(variantId, unitPrice, timeProvider.GetUtcNow());

                if (result.IsSuccess)
                    metrics.RecordItemPriceSynced();
                else
                    logger.CartItemPriceSyncFailed(cart.Id, variantId, result.Error.Message);
            }

            await context.SaveChangesAsync(cancellationToken);

            if (carts.Count < batchSize)
                break;

            offset += batchSize;
            context.ClearChangeTracker();
        }
    }
}