using CartService.Application.Caching;
using CartService.Application.Dtos;
using CartService.Application.Interfaces.Data;
using CartService.Application.Mapping;
using CartService.Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using migApp.Shared.Results;
using ZiggyCreatures.Caching.Fusion;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Application.Features.Queries.GetCart;

public sealed class GetCartQueryHandler(
    IAppDbContext context,
    IFusionCache cache) : IRequestHandler<GetCartQuery, IResult<CartDto>>
{
    public async Task<IResult<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await cache.GetOrSetAsync(
            CacheKeys.CartByBuyerId(request.BuyerId),
            async ct => await context.Carts
                .AsNoTracking()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.BuyerId == request.BuyerId, ct),
            token: cancellationToken);

        return cart is null ?
            Fail<CartDto>(CartErrors.NotFound()) :
            Ok(cart.ToDto());
    }
}