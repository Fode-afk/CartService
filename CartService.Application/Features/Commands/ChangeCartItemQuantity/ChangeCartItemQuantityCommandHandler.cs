using CartService.Application.Interfaces.Data;
using CartService.Domain.Errors;
using CartService.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Application.Features.Commands.ChangeCartItemQuantity;

public sealed class ChangeCartItemQuantityCommandHandler(
    IAppDbContext context,
    TimeProvider timeProvider) : IRequestHandler<ChangeCartItemQuantityCommand, IResult>
{
    public async Task<IResult> Handle(ChangeCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.BuyerId == request.BuyerId, cancellationToken);

        if (cart == null)
            return Fail(CartErrors.NotFound());

        var quantityResult = Quantity.Create(request.Quantity);

        if (quantityResult.IsFailure)
            return quantityResult;

        var productVariantInventorySnapshot = await context.ProductVariantInventorySnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductVariantId == request.VariantId, cancellationToken);

        if (productVariantInventorySnapshot == null)
            return Fail(ProductVariantInventorySnapshotErrors.NotFound());

        var result = cart.ChangeQuantity(
            request.VariantId,
            quantityResult.Value,
            productVariantInventorySnapshot.AvailableQuantity,
            timeProvider.GetUtcNow());

        if (result.IsFailure)
            return result;

        await context.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}