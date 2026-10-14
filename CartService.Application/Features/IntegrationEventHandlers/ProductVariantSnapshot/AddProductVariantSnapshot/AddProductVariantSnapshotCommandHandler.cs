using CartService.Application.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantSnapshot.AddProductVariantSnapshot;

public sealed class AddProductVariantSnapshotCommandHandler(IAppDbContext context) : IRequestHandler<AddProductVariantSnapshotCommand>
{
    public async Task Handle(AddProductVariantSnapshotCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.ProductVariantSnapshots
            .AnyAsync(x => x.ProductVariantId == request.ProductVariantId, cancellationToken);

        if (exists)
            return;

        context.ProductVariantSnapshots.Add(
            new Domain.Snapshots.ProductVariantSnapshot
            {
                ProductVariantId = request.ProductVariantId,
                ProductId = request.ProductId           
            });

        await context.SaveChangesAsync(cancellationToken);
    }
}