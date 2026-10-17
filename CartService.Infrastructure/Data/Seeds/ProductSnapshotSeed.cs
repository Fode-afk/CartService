using CartService.Domain.Snapshots;

namespace CartService.Infrastructure.Data.Seeds;

internal static class ProductSnapshotSeed
{
    public static readonly List<ProductSnapshot> Data =
    [
        new()
        {
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            VendorId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            IsVisiblePublicly = true,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            VendorId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            IsVisiblePublicly = true,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
            VendorId = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            IsVisiblePublicly = true,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        }
    ];
}