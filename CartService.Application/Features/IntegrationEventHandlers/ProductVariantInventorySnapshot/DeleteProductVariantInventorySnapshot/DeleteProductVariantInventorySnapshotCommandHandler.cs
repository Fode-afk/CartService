using CartService.Application.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantInventorySnapshot.DeleteProductVariantInventorySnapshot;

public sealed class DeleteProductVariantInventorySnapshotCommandHandler(
    IAppDbContext context) : IRequestHandler<DeleteProductVariantInventorySnapshotCommand>
{
    public async Task Handle(DeleteProductVariantInventorySnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductVariantInventorySnapshots
          .FirstOrDefaultAsync(p => p.ProductVariantId == request.ProductVariantId, cancellationToken);

        if (snapshot is null)
            return;

        context.ProductVariantInventorySnapshots.Remove(snapshot);

        await context.SaveChangesAsync(cancellationToken);
    }
}
