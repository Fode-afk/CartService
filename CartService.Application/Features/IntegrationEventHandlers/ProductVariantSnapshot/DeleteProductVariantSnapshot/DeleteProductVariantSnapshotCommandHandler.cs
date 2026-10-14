using CartService.Application.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantSnapshot.DeleteProductVariantSnapshot;

public sealed class DeleteProductVariantSnapshotCommandHandler(IAppDbContext context) : IRequestHandler<DeleteProductVariantSnapshotCommand>
{
    public async Task Handle(DeleteProductVariantSnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductVariantSnapshots
            .FirstOrDefaultAsync(v => v.ProductVariantId == request.ProductVariantId, cancellationToken);

        if (snapshot is null)
            return;

        context.ProductVariantSnapshots.Remove(snapshot);

        await context.SaveChangesAsync(cancellationToken);
    }
}