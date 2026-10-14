using migApp.Shared.Results;

namespace CartService.Domain.Errors;

public static class ProductVariantSnapshotErrors
{
    public static Error NotFound() =>
        Error.NotFound(ProductVariantSnapshotErrorCodes.NotFound,
            "Product variant not found.");

}

public static class ProductVariantSnapshotErrorCodes
{
    public const string NotFound = "ProductVariant.NotFound";
}
