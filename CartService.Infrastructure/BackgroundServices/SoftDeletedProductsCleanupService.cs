using CartService.Application.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CartService.Infrastructure.BackgroundServices;

public sealed class SoftDeletedProductsCleanupService(
    IServiceScopeFactory scopeFactory,
    ILogger<SoftDeletedProductsCleanupService> logger,
    TimeProvider timeProvider,
    SoftDeletedProductsCleanupOptions options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(options.Interval, timeProvider);

        await CleanupAsync(stoppingToken);

        while (await timer.WaitForNextTickAsync(stoppingToken))
            await CleanupAsync(stoppingToken);
    }

    public async Task CleanupAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var cutoff = timeProvider.GetUtcNow() - options.RetentionPeriod;
        long totalDeleted = 0;

        try
        {
            long deletedInBatch;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                var ids = await context.Carts
                    .IgnoreQueryFilters()
                    .Where(p => p.DeletedAt != null && p.DeletedAt <= cutoff)
                    .OrderBy(p => p.Id)
                    .Select(p => p.Id)
                    .Take(options.BatchSize)
                    .ToListAsync(cancellationToken);

                if (ids.Count == 0)
                {
                    deletedInBatch = 0;
                    break;
                }

                deletedInBatch = await context.Carts
                    .IgnoreQueryFilters()
                    .Where(p => ids.Contains(p.Id))
                    .ExecuteDeleteAsync(cancellationToken);
                totalDeleted += deletedInBatch;

                if (deletedInBatch > 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
                }
            }
            while (deletedInBatch == options.BatchSize);

            if (totalDeleted > 0)
            {
                logger.LogInformation(
                    "Cleanup soft-deleted records: removed {Count} records older than {Cutoff:u}",
                    totalDeleted, cutoff);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Cleanup soft-deleted records failed after removing {Count} records", totalDeleted);
        }
    }
}
