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
            PriceId = Guid.Parse("44000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            IsBase = true,
            AmountMinor = 1999,
            Currency = Currency.BYN,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            PriceId = Guid.Parse("56000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            IsBase = false,
            AmountMinor = 5000,
            Currency = Currency.USD,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            PriceId = Guid.Parse("59000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            IsBase = false,
            AmountMinor = 4600,
            Currency = Currency.EUR,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {PriceId = Guid.Parse("57000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            IsBase = true,
            AmountMinor = 2499,
            Currency = Currency.EUR,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            PriceId = Guid.Parse("54000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            IsBase = false,
            AmountMinor = 8500,
            Currency = Currency.BYN,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        // Hoodie: 39.99 USD, 49.99 USD
        new()
        {
            PriceId = Guid.Parse("46000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            IsBase = true,
            AmountMinor = 3999,
            Currency = Currency.BYN,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            PriceId = Guid.Parse("47000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            IsBase = true,
            AmountMinor = 4999,
            Currency = Currency.USD,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },

        // Sneakers: 59.99 USD, 79.99 USD
        new()
        {
            PriceId = Guid.Parse("48000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            IsBase = true,
            AmountMinor = 5999,
            Currency = Currency.RUB,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        },
        new()
        {
            PriceId = Guid.Parse("49000000-0000-0000-0000-000000000001"),
            ProductVariantId = Guid.Parse("30000000-0000-0000-0000-000000000006"),
            IsBase = true,
            AmountMinor = 7999,
            Currency = Currency.BYN,
            UpdatedAt = DateTimeOffset.UtcNow,
            Version = 1
        }
    ];
}
