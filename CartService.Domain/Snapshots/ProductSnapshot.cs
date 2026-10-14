namespace CartService.Domain.Snapshots;

public sealed class ProductSnapshot
{
    public Guid ProductId { get; set; }
    public Guid VendorId { get; set; }
    public bool IsVisiblePublicly { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}
