using CartService.Domain.Snapshots;

namespace CartService.Infrastructure.Data.Seeds;

internal static class ProductVariantSnapshotSeed
{
    public static readonly List<ProductVariantSnapshot> Data =
    [
         // T-Shirt
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        },

        // Hoodie
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000002")
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000002")
        },

        // Sneakers
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000003")
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000006"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000003")
        }
    ];
}
