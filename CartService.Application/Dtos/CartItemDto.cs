namespace CartService.Application.Dtos;

public sealed record CartItemDto(
    Guid VariantId,
    Guid ProductId,
    Guid VendorId,
    int Quantity,
    long UnitPriceMinor,
    long LineTotalMinor,
    bool IsAvailable,
    IReadOnlyList<string> SuspensionReasons);