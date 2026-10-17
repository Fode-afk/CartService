namespace CartService.Domain.Snapshots;

public sealed class ProductSnapshot
{
    public Guid ProductId { get; init; }
    public Guid VendorId { get; init; }
    public bool IsVisiblePublicly { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}
