using CartService.Domain.Snapshots;
using migApp.Shared.Enums.Vendors;

namespace CartService.Infrastructure.Data.Seeds;

internal static class VendorSnapshotSeed
{
    public static readonly List<VendorSnapshot> Data =
    [
        // =========================
        // ACTIVE + VERIFIED
        // =========================

        new()
        {
            VendorId = Guid.Parse("a1000000-0000-0000-0000-000000000001"),
            IsActive = true,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-60),
            Version = 1
        },

        new()
        {
            VendorId = Guid.Parse("a1000000-0000-0000-0000-000000000002"),
            IsActive = true,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-12),
            Version = 2
        },

        // =========================
        // ACTIVE BUT NOT VERIFIED
        // =========================

        new()
        {
            VendorId = Guid.Parse("b2000000-0000-0000-0000-000000000001"),
            IsActive = false,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-20),
            Version = 1
        },

        new()
        {
            VendorId = Guid.Parse("b2000000-0000-0000-0000-000000000002"),
            IsActive = false,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-3),
            Version = 3
        },

        // =========================
        // BLOCKED / INACTIVE
        // =========================

        new()
        {
            VendorId = Guid.Parse("c3000000-0000-0000-0000-000000000001"),
            IsActive = false,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-100),
            Version = 5
        },

        new()
        {
            VendorId = Guid.Parse("c3000000-0000-0000-0000-000000000002"),
            IsActive = false,
            UpdatedAt = DateTimeOffset.UtcNow.AddDays(-45),
            Version = 2
        },

        // =========================
        // PENDING / MODERATION
        // =========================

        new()
        {
            VendorId = Guid.Parse("d4000000-0000-0000-0000-000000000001"),
            IsActive = false,
            UpdatedAt = DateTimeOffset.UtcNow.AddHours(-12),
            Version = 1
        },

        new()
        {
            VendorId = Guid.Parse("d4000000-0000-0000-0000-000000000002"),
            IsActive = false,
            UpdatedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Version = 4
        }
    ];
}