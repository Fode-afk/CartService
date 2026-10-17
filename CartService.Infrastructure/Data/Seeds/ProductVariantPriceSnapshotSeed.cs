using CartService.Domain.Snapshots;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Infrastructure.Data.Seeds;

internal static class ProductVariantPriceSnapshotSeed
{
    public static readonly List<ProductVariantPriceSnapshot> Data =
    [
       // T-Shirt: 19.99 USD, 24.99 USD
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            UnitPrice = Money.FromMinor(1999, Currency.USD).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            UnitPrice = Money.FromMinor(2499, Currency.USD).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },

        // Hoodie: 39.99 USD, 49.99 USD
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            UnitPrice = Money.FromMinor(3999, Currency.USD).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            UnitPrice = Money.FromMinor(4999, Currency.USD).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },

        // Sneakers: 59.99 USD, 79.99 USD
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
            UnitPrice = Money.FromMinor(5999, Currency.USD).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000006"),
            ProductId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
            UnitPrice = Money.FromMinor(7999, Currency.USD).Value,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        }
    ];
}
