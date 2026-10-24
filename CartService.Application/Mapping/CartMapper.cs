using CartService.Application.Dtos;
using CartService.Domain.Models;
using migApp.Shared.Domain.Extensions;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Mapping;

internal static class CartMapper
{
    public static CartDto ToDto(
        this Cart cart,
        IReadOnlyDictionary<Guid, Money> unitPrices,
        IReadOnlyDictionary<Currency, decimal> rates,
        Currency targetCurrency)
    {
        var items = new List<CartItemDto>(cart.Items.Count);
        long subtotal = 0;

        foreach (var item in cart.Items)
        {
            var hasPrice = unitPrices.TryGetValue(item.VariantId, out var price);

            var unit = hasPrice ? ToTargetMinor(price!, targetCurrency, rates) : 0L;

            var line = checked(unit * item.Quantity.Value);

            var isAvailable = item.IsAvailable && hasPrice;

            if (isAvailable)
                subtotal = checked(subtotal + line);

            items.Add(new CartItemDto(
                item.VariantId,
                item.ProductId,
                item.VendorId,
                item.Quantity.Value,
                unit,
                line,
                isAvailable,
                [.. item.SuspensionReasons.Select(r => r.Reason.ToString())]));
        }

        var available = items.Where(x => x.IsAvailable).ToList();

        return new CartDto(
            cart.Id,
            items,
            new CartTotalsDto(
                DistinctItems: available.Count,
                TotalQuantity: available.Sum(x => x.Quantity),
                SubtotalMinor: subtotal,
                Currency: targetCurrency.Code));
    }

    private static long ToTargetMinor(
        Money amount,
        Currency targetCurrency,
        IReadOnlyDictionary<Currency, decimal> rates)
    {
        if (amount.Currency == targetCurrency)
            return amount.ToMinor();

        if (!rates.TryGetValue(amount.Currency, out var rate))
            throw new InvalidOperationException(
                $"Exchange rate {amount.Currency.Code} -> {targetCurrency.Code} is missing.");

        return amount.ConvertToMinor(targetCurrency, rate);
    }
}