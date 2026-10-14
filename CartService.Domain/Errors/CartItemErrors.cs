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
}

public static class CartItemErrorCodes
{
    public const string NotFound = "CartItem.NotFound";
    public const string Unavailable = "CartItem.Unavailable";
}