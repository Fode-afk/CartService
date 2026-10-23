using CartService.Application.Dtos;
using CartService.Domain.Models;
using migApp.Shared.Domain.Extensions;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Mapping;

internal static class CartMapper
{
    public static CartDto ToDto(
        this Cart cart,
        IReadOnlyDictionary<Currency, decimal> rates,
        Currency targetCurrency)
    {
        var items = new List<CartItemDto>(cart.Items.Count);
        long subtotal = 0;

        foreach (var item in cart.Items)
        {
            var unit = ToTargetMinor(item.UnitPrice, targetCurrency, rates);
            var added = ToTargetMinor(item.PriceWhenAdded, targetCurrency, rates);

            var line = checked(unit * item.Quantity.Value);

            if (item.IsAvailable)
                subtotal = checked(subtotal + line);

            items.Add(new CartItemDto(
                item.VariantId,
                item.ProductId,
                item.VendorId,
                item.Quantity.Value,
                unit,
                line,
                added,
                item.IsPriceChangedSinceAdded,
                item.IsAvailable,
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