using migApp.Shared.Results;

namespace CartService.Domain.Errors;

public static class QuantityErrors
{
    public static Error Negative() =>
        Error.InvalidArgument(QuantityErrorCodes.Negative,
            "Quantity cannot be negative.");
}

public static class QuantityErrorCodes
{
    public const string Negative = "Quantity.Negative";
}