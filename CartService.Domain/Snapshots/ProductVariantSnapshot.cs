namespace CartService.Domain.Snapshots;

public sealed class ProductVariantSnapshot
{
    public Guid ProductVariantId { get; set; }
    public Guid ProductId { get; set; }
}