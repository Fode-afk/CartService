using CartService.Application.Dtos;
using CartService.Domain.Models;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Mapping;

internal static class CartMapper
{
    public static CartDto ToDto(
        this Cart cart,
        decimal exchangeRate,
        Currency targetCurrency)
    {
        var items = new List<CartItemDto>(cart.Items.Count);
        long subtotal = 0;

        foreach (var item in cart.Items)
        {
            var unit = ConvertUsdToTargetMinor(
                item.UnitPrice.ToMinor(),
                targetCurrency,
                exchangeRate);
            var added = ConvertUsdToTargetMinor(
                item.PriceWhenAdded.ToMinor(),
                targetCurrency,
                exchangeRate);

            var line = unit * item.Quantity.Value;

            if (item.IsAvailable) 
                subtotal += line;

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
                [..item.SuspensionReasons.Select(r => r.Reason.ToString())]));
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

    private static long ConvertUsdToTargetMinor(
        long usdAmountMinor,
        Currency targetCurrency,
        decimal exchangeRate)
    {
        if (targetCurrency == Currency.USD)
            return usdAmountMinor;

        var usdMajor = usdAmountMinor / Currency.USD.MinorFactor;
        var targetMinor = Math.Round(
            usdMajor * exchangeRate * targetCurrency.MinorFactor,
            0,
            MidpointRounding.AwayFromZero);

        return decimal.ToInt64(targetMinor);
    }
}