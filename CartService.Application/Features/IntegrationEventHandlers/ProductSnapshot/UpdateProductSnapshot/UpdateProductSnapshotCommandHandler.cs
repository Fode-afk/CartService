using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Jobs;
using CartService.Application.Interfaces.Metrics;
using CartService.Domain.Exceptions;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;

public sealed class UpdateProductSnapshotCommandHandler(
    IAppDbContext context,
    IBackgroundJobClient backgroundJobClient,
    ICartMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<UpdateProductSnapshotCommand>
{
    public async Task Handle(UpdateProductSnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductSnapshots
            .FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);

        if (snapshot is null)
        {
            metrics.RecordSnapshotNotFound("Product");
            throw new SnapshotNotFoundException("Product", request.ProductId);
        }

        if (request.Version <= snapshot.Version)
        {
            metrics.RecordSnapshotOutdated(nameof(UpdateProductSnapshotCommand));
            return;
        }

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        snapshot.IsVisiblePublicly = request.IsVisiblePublicly;
        snapshot.UpdatedAt = timeProvider.GetUtcNow();
        snapshot.Version = request.Version;

        await context.SaveChangesAsync(cancellationToken);

        backgroundJobClient.Enqueue<ISuspendProductCartItemsJob>(
           job => job.Execute(request.ProductId, isVisiblePublicly: false, CancellationToken.None));

        await transaction.CommitAsync(cancellationToken);
    }
}