using CartService.Domain.ValueObjects;

namespace CartService.Domain.Snapshots;

public sealed class ProductVariantInventorySnapshot
{
    public Guid ProductVariantId { get; init; }
    public required Quantity AvailableQuantity { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}
