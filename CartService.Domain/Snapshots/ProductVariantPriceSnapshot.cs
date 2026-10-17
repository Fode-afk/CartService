using migApp.Shared.Domain.ValueObjects;

namespace CartService.Domain.Snapshots;

public sealed class ProductVariantPriceSnapshot
{
    public Guid ProductVariantId { get; init; }
    public Guid ProductId { get; init; }
    public required Money UnitPrice { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}