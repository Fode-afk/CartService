using CartService.Domain.Snapshots;

namespace CartService.Infrastructure.Data.Seeds;

internal static class BrandSnapshotSeed
{
    public static readonly List<BrandSnapshot> Data =
    [
        // =========================
        // ACTIVE BRANDS
        // =========================

        new()
        {
            BrandId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            IsAssignable = true,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-30),
            Version = 1
        },

        // =========================
        // INACTIVE BRANDS
        // =========================

        new()
        {
            BrandId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            IsAssignable = false,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-5),
            Version = 1
        }
    ];
}
