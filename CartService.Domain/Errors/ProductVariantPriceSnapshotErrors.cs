using migApp.Shared.Results;

namespace CartService.Domain.Errors;

public static class ProductVariantPriceSnapshotErrors
{
    public static Error NotFound() =>
        Error.NotFound(ProductVariantPriceSnapshotErrorCodes.NotFound,
            "Product variant price not found.");
}

public static class ProductVariantPriceSnapshotErrorCodes
{
    public const string NotFound = "ProductVariantPriceSnapshot.NotFound";
}