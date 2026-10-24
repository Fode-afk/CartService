using CartService.Application.Interfaces.Data;
using CartService.Domain.Errors;
using CartService.Domain.Snapshots;
using CartService.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using migApp.Shared.Domain.ValueObjects;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Application.Features.Commands.AddCartItem;

public sealed class AddCartItemCommandHandler(
    IAppDbContext context,
    TimeProvider timeProvider) : IRequestHandler<AddCartItemCommand, IResult>
{
    public async Task<IResult> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.BuyerId == request.BuyerId, cancellationToken);

        if (cart == null)
            return Fail(CartErrors.NotFound());

        var variantSnapshot = await context.ProductVariantSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.ProductVariantId == request.VariantId, cancellationToken);

        if (variantSnapshot == null)
            return Fail(ProductVariantSnapshotErrors.NotFound());

        var productSnapshot = await context.ProductSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == variantSnapshot.ProductId, cancellationToken);

        if (productSnapshot == null || !productSnapshot.IsVisiblePublicly)
            return Fail(ProductSnapshotErrors.NotFound());

        var currencyResult = Currency.Create(request.Currency);

        if (currencyResult.IsFailure)
            return currencyResult;

        var hasProductVariantPriceSnapshot = await context.ProductVariantPriceSnapshots
            .AnyAsync(p => p.ProductVariantId == request.VariantId && p.IsBase, cancellationToken);

        if (!hasProductVariantPriceSnapshot)
            return Fail(ProductVariantPriceSnapshotErrors.NotFound());

        var quantityResult = Quantity.Create(request.Quantity);

        if (quantityResult.IsFailure)
            return quantityResult;

        var productVariantInventorySnapshot = await context.ProductVariantInventorySnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductVariantId == request.VariantId, cancellationToken);

        if (productVariantInventorySnapshot == null)
            return Fail(ProductVariantInventorySnapshotErrors.NotFound());

        var result = cart.AddItem(
            request.VariantId,
            productSnapshot.ProductId,
            productSnapshot.VendorId,
            quantityResult.Value,
            productVariantInventorySnapshot.AvailableQuantity,
            timeProvider.GetUtcNow());

        if (result.IsFailure)
            return result;

        await context.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}