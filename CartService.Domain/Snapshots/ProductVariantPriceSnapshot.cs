using migApp.Shared.Domain.ValueObjects;

namespace CartService.Domain.Snapshots;

public sealed class ProductVariantPriceSnapshot
{
    public Guid ProductVariantId { get; set; }
    public Guid ProductId { get; set; }
    public required Money UnitPrice { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}