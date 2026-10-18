using CartService.Application.Interfaces.Services;
using migApp.Shared.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace CartService.Infrastructure.Services;

internal sealed class ExchangeRateService(IFusionCache cache, ICurrencyService currencyService) : IExchangeRateService
{
    private static string ExchangeRateByCurrency(string currency) => $"ExchangeRate:{currency}";
    private static readonly FusionCacheEntryOptions RateOptions = new()
    {
        Duration = TimeSpan.FromHours(1),
        IsFailSafeEnabled = true,
        FailSafeMaxDuration = TimeSpan.FromHours(24),
        AllowStaleOnReadOnly = true
    };

    public async Task<decimal?> GetExchangeRateAsync(
        Currency sourceCurrency, 
        Currency targetCurrency,
        CancellationToken cancellationToken = default)
    {
        if (sourceCurrency == targetCurrency)
            return 1m;

        var sourceCurrencyRate = await cache.GetOrDefaultAsync<decimal?>(
            ExchangeRateByCurrency(sourceCurrency.Code),
            options: RateOptions,
            token: cancellationToken);

        var targetCurrencyRate = await cache.GetOrDefaultAsync<decimal?>(
            ExchangeRateByCurrency(targetCurrency.Code),
            options: RateOptions,
            token: cancellationToken);

        if (sourceCurrencyRate is not > 0 || targetCurrencyRate is not > 0)
            return await currencyService.GetExchangeRateAsync(
                sourceCurrency.Code,
                targetCurrency.Code,
                cancellationToken);

        return targetCurrencyRate.Value / sourceCurrencyRate.Value;
    }

    public async Task UpdateExchangeRatesAsync(
        Dictionary<string, decimal> rates,
        CancellationToken cancellationToken = default) =>
        await Task.WhenAll(
            rates.Select(async rate =>
                await cache.SetAsync(
                    ExchangeRateByCurrency(rate.Key),
                    rate.Value,
                    options: RateOptions,
                    token: cancellationToken)));
}