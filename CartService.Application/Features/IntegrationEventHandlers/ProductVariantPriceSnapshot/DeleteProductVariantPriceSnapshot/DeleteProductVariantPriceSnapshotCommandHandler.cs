using CartService.Application.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantPriceSnapshot.DeleteProductVariantPriceSnapshot;

public sealed class DeleteProductVariantPriceSnapshotCommandHandler(IAppDbContext context) : IRequestHandler<DeleteProductVariantPriceSnapshotCommand>
{
    public async Task Handle(DeleteProductVariantPriceSnapshotCommand request, CancellationToken cancellationToken)
    {
        var snapshot = await context.ProductVariantPriceSnapshots
           .FirstOrDefaultAsync(p => p.ProductVariantId == request.ProductVariantId, cancellationToken);

        if (snapshot is null)
            return;

        context.ProductVariantPriceSnapshots.Remove(snapshot);

        await context.SaveChangesAsync(cancellationToken);
    }
}
