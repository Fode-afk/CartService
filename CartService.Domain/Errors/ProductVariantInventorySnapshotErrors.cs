using migApp.Shared.Results;

namespace CartService.Domain.Errors;

public static class ProductVariantInventorySnapshotErrors
{
    public static Error NotFound() =>
        Error.NotFound(ProductVariantInventorySnapshotErrorCodes.NotFound,
            "Product variant inventory snapshot not found.");
}

public static class ProductVariantInventorySnapshotErrorCodes
{
    public const string NotFound = "ProductVariantInventorySnapshot.NotFound";
}