using migApp.Shared.Results;

namespace CartService.Domain.Errors;

public static class ProductSnapshotErrors
{
    public static Error NotFound() => 
        Error.NotFound(ProductSnapshotErrorCodes.NotFound,
            "Product snapshot not found.");
}

public static class ProductSnapshotErrorCodes
{
    public const string NotFound = "ProductSnapshot.NotFound";
}