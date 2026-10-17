using CartService.Domain.Snapshots;
using CartService.Domain.ValueObjects;

namespace CartService.Infrastructure.Data.Seeds;

internal static class ProductVariantInventorySnapshotSeed
{
    public static readonly List<ProductVariantInventorySnapshot> Data =
    [
        // T-Shirt
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            AvailableQuantity = Quantity.Create(100).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            AvailableQuantity = Quantity.Create(75).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },

        // Hoodie
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            AvailableQuantity = Quantity.Create(50).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            AvailableQuantity = Quantity.Create(30).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },

        // Sneakers
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            AvailableQuantity = Quantity.Create(20).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000006"),
            AvailableQuantity = Quantity.Create(0).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        }
    ];
}