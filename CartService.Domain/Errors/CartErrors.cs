using migApp.Shared.Results;

namespace CartService.Domain.Errors;

public static class CartErrors
{
    public static Error DistinctItemsLimitReached(int maxDistinctItems) =>
        Error.InvalidArgument(CartErrorCodes.DistinctItemsLimitReached,
            $"Distinct items limit: {maxDistinctItems} reached.");

    public static Error NotFound() =>
        Error.NotFound(CartErrorCodes.NotFound,
            "Cart not found.");
}

public static class CartErrorCodes
{
    public const string DistinctItemsLimitReached = "Cart.DistinctItemsLimitReached";
    public const string NotFound = "Cart.NotFound";
    public const string InvalidId = "Cart.InvalidId";
}
