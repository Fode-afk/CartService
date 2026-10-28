using CartService.Application.Dtos;
using CartService.Domain.Models;

namespace CartService.Application.Mapping;

internal static class CartMapper
{
    public static CartDto ToDto(this Cart cart) =>
        new(
            cart.Id,
            [..cart.Items.Select(i => i.ToDto())]);

    public static CartItemDto ToDto(this CartItem cartItem) =>
        new(
            cartItem.VariantId,
            cartItem.ProductId,
            cartItem.VendorId,
            cartItem.Quantity.Value,
            cartItem.IsAvailable,
            [.. cartItem.SuspensionReasons.Select(r => r.Reason.ToString())]);
}