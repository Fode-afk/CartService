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
                .FirstOrDefaultAsync(c => c.BuyerId == request.BuyerId, ct),
            token: cancellationToken);

        if (cart is null)
            return Fail<CartDto>(CartErrors.NotFound());

        var variantIds = cart.Items.Select(i => i.VariantId).Distinct().ToList();

        var snapshots = await context.ProductVariantPriceSnapshots
            .AsNoTracking()
            .Where(p =>
                variantIds.Contains(p.ProductVariantId) &&
                (p.IsBase || p.Currency == targetCurrency))
            .ToListAsync(cancellationToken);

        var unitPrices = new Dictionary<Guid, Money>();

        foreach (var group in snapshots.GroupBy(p => p.ProductVariantId))
        {
            var chosen = group.FirstOrDefault(p => !p.IsBase && p.UnitPrice.Currency == targetCurrency)
                ?? group.FirstOrDefault(p => p.IsBase);

            if (chosen is not null)
                unitPrices[group.Key] = chosen.UnitPrice;
        }

        var sourceCurrencies = unitPrices.Values
            .Select(m => m.Currency)
            .Where(c => c != targetCurrency)
            .Distinct()
            .ToList();

        var rates = new Dictionary<Currency, decimal>(sourceCurrencies.Count);

        foreach (var source in sourceCurrencies)
        {
            var rate = await exchangeRateService.GetExchangeRateAsync(
                source,
                targetCurrency,
                cancellationToken);

            if (rate is null)
            {
                logger.LogWarning(
                    "Exchange rate {Source}→{Target} unavailable, cannot build cart for buyer {BuyerId}",
                    source.Code,
                    targetCurrency.Code,
                    request.BuyerId);

                return Fail<CartDto>(CartErrors.ExchangeRateUnavailable());
            }

            rates.Add(source, rate.Value);
        }

        return Ok(cart.ToDto(unitPrices, rates, targetCurrency));
    }
}