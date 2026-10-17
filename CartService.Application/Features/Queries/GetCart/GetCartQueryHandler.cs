using CartService.Application.Caching;
using CartService.Application.Dtos;
using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Services;
using CartService.Application.Mapping;
using CartService.Domain.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using migApp.Shared.Domain.ValueObjects;
using migApp.Shared.Results;
using ZiggyCreatures.Caching.Fusion;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Application.Features.Queries.GetCart;

public sealed class GetCartQueryHandler(
    IAppDbContext context,
    IFusionCache cache,
    IExchangeRateService exchangeRateService,
    ILogger<GetCartQueryHandler> logger) : IRequestHandler<GetCartQuery, IResult<CartDto>>
{
    public async Task<IResult<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var currencyResult = Currency.Create(request.Currency);
        if (currencyResult.IsFailure)
            return Fail<CartDto>(currencyResult.Error);

        var targetCurrency = currencyResult.Value;

        var cart = await cache.GetOrSetAsync(
            CacheKeys.CartByBuyerId(request.BuyerId),
            async ct => await context.Carts
                .AsNoTracking()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.BuyerId == request.BuyerId, cancellationToken),
            token: cancellationToken);

        if (cart is null)
            return Fail<CartDto>(CartErrors.NotFound());

        var rate = await exchangeRateService.GetExchangeRateAsync(
            Currency.USD,
            targetCurrency, cancellationToken);

        if (rate is null)
        {
            logger.LogWarning(
                "Exchange rate USD→{Currency} unavailable, returning cart in USD for buyer {BuyerId}",
                targetCurrency.Code, request.BuyerId);

            return Ok(cart.ToDto(1m, Currency.USD));
        }

        return Ok(cart.ToDto(rate.Value, targetCurrency));
    }
}