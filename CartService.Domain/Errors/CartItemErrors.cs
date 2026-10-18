using CartService.Domain.ValueObjects;
using migApp.Shared.Results;

namespace CartService.Domain.Errors;

public static class CartItemErrors
{
    public static Error NotFound() =>
        Error.NotFound(CartItemErrorCodes.NotFound,
            "Cart item not found.");

    public static Error Unavailable() =>
        Error.Internal(CartItemErrorCodes.Unavailable,
            "Cart item is unaviable.");

    public static Error MaxQuantityReached(Quantity maxQuantity) =>
        Error.InvalidArgument(CartItemErrorCodes.MaxQuantityReached,
            $"Max quantity: {maxQuantity} for cart item reached.");

    public static Error InvalidQuantity() =>
        Error.InvalidArgument(CartItemErrorCodes.InvalidQuantity,
            "Invalid quantity.");
}

public static class CartItemErrorCodes
{
    public const string NotFound = "CartItem.NotFound";
    public const string Unavailable = "CartItem.Unavailable";
    public const string MaxQuantityReached = "CartItem.MaxQuantityReached";
    public const string InvalidQuantity = "CartItem.InvalidQuantity";
}