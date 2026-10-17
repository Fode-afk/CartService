using CartService.Application.Interfaces.Data;
using CartService.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductVariantInventorySnapshot.AddProductVariantInventorySnapshot;

public sealed class AddProductVariantInventorySnapshotCommandHandler(
    IAppDbContext context,
    ILogger<AddProductVariantInventorySnapshotCommandHandler> logger,
    TimeProvider timeProvider) : IRequestHandler<AddProductVariantInventorySnapshotCommand>
{
    public async Task Handle(AddProductVariantInventorySnapshotCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.ProductVariantInventorySnapshots
            .AnyAsync(x => x.ProductVariantId == request.ProductVariantId, cancellationToken);

        if (exists)
            return;

        var quantityResult = Quantity.Create(request.AvailableQuantity);

        if (quantityResult.IsFailure)
        {
            logger.LogWarning(
                "Invalid quantity in {Command}: ProductVariantId={ProductVariantId}, Version={Version}, Quantity={Quantity}, Error={@Error}",
                nameof(AddProductVariantInventorySnapshotCommand),
                request.ProductVariantId,
                request.Version,
                request.AvailableQuantity,
                quantityResult.Error);
        }

        context.ProductVariantInventorySnapshots.Add(
            new Domain.Snapshots.ProductVariantInventorySnapshot
            {
                ProductVariantId = request.ProductVariantId,
                AvailableQuantity = quantityResult.Value,
                UpdatedAt = timeProvider.GetUtcNow(),
                Version = request.Version
            });

        await context.SaveChangesAsync(cancellationToken);
    }
}
