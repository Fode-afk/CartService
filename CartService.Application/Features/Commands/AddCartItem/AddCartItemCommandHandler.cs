using CartService.Application.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;
using CartService.Domain.Errors;
using CartService.Domain.ValueObjects;

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

        var productVariantPriceSnapshot = await context.ProductVariantPriceSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductVariantId == variantSnapshot.ProductVariantId, cancellationToken);

        if (productVariantPriceSnapshot == null)
            return Fail(ProductVariantPriceSnapshotErrors.NotFound());

        var quantityResult = Quantity.Create(request.Quantity);

        if (quantityResult.IsFailure)
            return quantityResult;

        var result = cart.AddItem(
            request.VariantId,
            productSnapshot.ProductId,
            productSnapshot.VendorId,
            quantityResult.Value,
            productVariantPriceSnapshot.UnitPrice,
            timeProvider.GetUtcNow());

        if (result.IsFailure)
            return result;

        if (result.Value != null)
            context.CartItems.Add(result.Value);

        await context.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}