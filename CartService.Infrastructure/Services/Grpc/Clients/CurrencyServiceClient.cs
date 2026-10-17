using CurrencyService.Api.Grpc.V1.Protos;
using Polly;
using Proto = CurrencyService.Api.Grpc.V1.Protos;
using CartService.Application.Interfaces.Services;

namespace CartService.Infrastructure.Services.Grpc.Clients;

internal sealed class CurrencyServiceClient(
    Proto.CurrencyService.CurrencyServiceClient client,
    IAsyncPolicy circuitBreakerPolicy) : ICurrencyService
{
    public async Task<decimal?> GetExchangeRateAsync(string sourceCurrency, string targetCurrency, CancellationToken cancellationToken = default)
    {
        var result = await circuitBreakerPolicy.ExecuteAsync(async ct =>
            await client.GetExchangeRateAsync(
                new GetExchangeRateRequest
                {
                    CurrencyFrom = sourceCurrency,
                    CurrencyTo = targetCurrency
                }, cancellationToken: ct), cancellationToken);

        return decimal.TryParse(result.Rate, out var rate) ? rate : null;
    }
}