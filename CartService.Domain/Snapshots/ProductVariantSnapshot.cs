namespace CartService.Domain.Snapshots;

public sealed class ProductVariantSnapshot
{
    public Guid ProductVariantId { get; init; }
    public Guid ProductId { get; init; }
}