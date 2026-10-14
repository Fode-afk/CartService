using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Jobs;
using CartService.Application.Interfaces.Metrics;
using CartService.Application.Logging;
using CartService.Domain.Enums;
using CartService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CartService.Infrastructure.Jobs;

internal sealed class SuspendProductCartItemsJob(
    IAppDbContext context, 
    ICartMetrics metrics,
    ILogger<SuspendProductCartItemsJob> logger,
    TimeProvider timeProvider) : ISuspendProductCartItemsJob
{
    public async Task Execute(Guid productId, bool isVisiblePublicly, CancellationToken cancellationToken = default)
    {
        var suspensionReason = new CartItemSuspensionReason(SuspensionReason.Unpublished);

        const int batchSize = 100;
        var offset = 0;

        while (true)
        {
            var carts = await context.Carts
                .Include(c => c.Items)
                .Where(c => c.Items.Any(i => i.ProductId == productId))
                .OrderBy(p => p.Id)
                .Skip(offset)
                .Take(batchSize)
                .ToListAsync(cancellationToken);

            if (carts.Count == 0)
                break;

            foreach (var cart in carts)
            {
                if (!isVisiblePublicly)
                {
                    var result = cart.SuspendItems(     
                        productId,
                        suspensionReason,
                        timeProvider.GetUtcNow());

                    if (result.IsSuccess)
                        metrics.RecordCartItemsSuspended(suspensionReason.Reason.ToString());
                    else 
                        logger.CartItemsSuspendFailed(cart.Id, result.Error.Message);
                }
                else
                {
                    var result = cart.TryRestoreItems(
                        productId,
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
