using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Jobs;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.DeleteProductSnapshot;

public sealed class DeleteProductSnapshotCommandHandler(
    IAppDbContext context,
    IBackgroundJobClient backgroundJobClient) : IRequestHandler<DeleteProductSnapshotCommand>
{
    public async Task Handle(DeleteProductSnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductSnapshots
            .FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);

        if (snapshot is null)
            return;

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        context.ProductSnapshots.Remove(snapshot);

        await context.SaveChangesAsync(cancellationToken);

        backgroundJobClient.Enqueue<ISuspendProductCartItemsJob>(
            job => job.Execute(request.ProductId, isVisiblePublicly: false, CancellationToken.None));

        await transaction.CommitAsync(cancellationToken);
    }
}