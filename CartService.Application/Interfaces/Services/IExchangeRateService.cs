using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Interfaces.Services;

public interface IExchangeRateService
{
    Task<decimal?> GetExchangeRateAsync(
        Currency sourceCurrency,
        Currency targetCurrency,
        CancellationToken cancellationToken = default);

    Task UpdateExchangeRatesAsync(
        Dictionary<string, decimal> rates,
        CancellationToken cancellationToken = default);
}