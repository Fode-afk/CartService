using migApp.Shared.Domain.ValueObjects;

namespace CartService.Domain.Snapshots;

public sealed class ProductVariantPriceSnapshot
{
    public Guid PriceId { get; init; }
    public Guid ProductVariantId { get; init; }
    public bool IsBase { get; init; }
    public required Currency Currency { get; set; }
    public long AmountMinor { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }

    public Money UnitPrice => Money.FromMinor(AmountMinor, Currency).Value;
}