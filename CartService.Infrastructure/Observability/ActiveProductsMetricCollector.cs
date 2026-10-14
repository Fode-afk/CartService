using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Metrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using migApp.Shared.Enums.Products;

namespace CartService.Infrastructure.Observability;

public sealed class ActiveProductsMetricCollector(
    IServiceScopeFactory scopeFactory,
    ICartMetrics metrics,
    ILogger<ActiveProductsMetricCollector> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var context = scope.ServiceProvider
                    .GetRequiredService<IAppDbContext>();

                var count = await context.Carts
                    .CountAsync(p => p.ProductStatus == ProductStatus.Published, cancellationToken);

                metrics.SetActiveProductsCount(count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to collect active products count");
            }
        }
    }
}