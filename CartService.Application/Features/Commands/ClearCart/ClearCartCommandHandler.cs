using CartService.Application.Interfaces.Data;
using CartService.Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Application.Features.Commands.ClearCart;

public sealed class ClearCartCommandHandler(
    IAppDbContext context,
    TimeProvider timeProvider) : IRequestHandler<ClearCartCommand, IResult>
{
    public async Task<IResult> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.BuyerId == request.BuyerId, cancellationToken);

        if (cart == null)
            return Fail(CartErrors.NotFound());

        var result = cart.Clear(timeProvider.GetUtcNow());

        if (result.IsFailure)
            return result;

        await context.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}
