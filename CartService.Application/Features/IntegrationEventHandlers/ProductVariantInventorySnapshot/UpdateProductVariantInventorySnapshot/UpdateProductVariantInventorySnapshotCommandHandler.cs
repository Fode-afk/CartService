using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Jobs;
using CartService.Application.Interfaces.Metrics;
using CartService.Domain.Exceptions;
using CartService.Domain.ValueObjects;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantInventorySnapshot.UpdateProductVariantInventorySnapshot;

public sealed class UpdateProductVariantInventorySnapshotCommandHandler(
    IAppDbContext context,
    IBackgroundJobClient backgroundJobClient,
    ICartMetrics metrics,
    ILogger<UpdateProductVariantInventorySnapshotCommandHandler> logger,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductVariantInventorySnapshotCommand>
{
    public async Task Handle(UpdateProductVariantInventorySnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductVariantInventorySnapshots
            .FirstOrDefaultAsync(p => p.ProductVariantId == request.ProductVariantId, cancellationToken);

        if (snapshot == null)
        {
            metrics.RecordSnapshotNotFound("ProductVariantInventory");
            throw new SnapshotNotFoundException("ProductVariantInventory", request.ProductVariantId);
        }

        if (request.Version <= snapshot.Version)
        {
            metrics.RecordSnapshotOutdated(nameof(UpdateProductVariantInventorySnapshotCommand));
            return;
        }

        var quantityResult = Quantity.Create(request.AvailableQuantity);

        if (quantityResult.IsFailure)
        {
            logger.LogWarning(
                "Invalid quantity in {Command}: ProductVariantId={ProductVariantId}, Version={Version}, Quantity={Quantity}, Error={@Error}",
                nameof(UpdateProductVariantInventorySnapshotCommand),
                request.ProductVariantId,
                request.Version,
                request.AvailableQuantity,
                quantityResult.Error);
        }

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        snapshot.AvailableQuantity = quantityResult.Value;
        snapshot.Version = request.Version;
        snapshot.UpdatedAt = timeProvider.GetUtcNow();

        await context.SaveChangesAsync(cancellationToken);

        backgroundJobClient.Enqueue<ISuspendInventoryCartItemJob>(
          job => job.Execute(request.ProductVariantId, snapshot.AvailableQuantity.Value > 0, CancellationToken.None));

        await transaction.CommitAsync(cancellationToken);
    }
}
