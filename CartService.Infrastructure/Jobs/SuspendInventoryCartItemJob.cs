using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Jobs;
using CartService.Application.Interfaces.Metrics;
using CartService.Application.Logging;
using CartService.Domain.Enums;
using CartService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CartService.Infrastructure.Jobs;

internal sealed class SuspendInventoryCartItemJob(
    IAppDbContext context,
    ICartMetrics metrics,
    ILogger<SuspendInventoryCartItemJob> logger,
    TimeProvider timeProvider) : ISuspendInventoryCartItemJob
{
    public async Task Execute(Guid variantId, bool inStock, CancellationToken cancellationToken = default)
    {
        var suspensionReason = new CartItemSuspensionReason(SuspensionReason.OutOfStock);

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
                if (!inStock)
                {
                    var result = cart.SuspendItem(
                        variantId,
                        suspensionReason,
                        timeProvider.GetUtcNow());

                    if (result.IsSuccess)
                        metrics.RecordCartItemsSuspended(suspensionReason.Reason.ToString());
                    else
                        logger.CartItemsSuspendFailed(cart.Id, result.Error.Message);
                }
                else
                {
                    var result = cart.TryRestoreItem(
                        variantId,
                        suspensionReason,
                        timeProvider.GetUtcNow());

                    if (result.IsSuccess)
                        metrics.RecordCartItemsRestored();
                    else
                        logger.CartItemsRestoreFailed(cart.Id, result.Error.Message);
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            if (carts.Count < batchSize)
                break;

            offset += batchSize;
            context.ClearChangeTracker();
        }
    }
}
