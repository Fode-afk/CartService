using CartService.Infrastructure.Data.Seeds;

namespace CartService.Infrastructure.Data;

internal static class DbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.VendorSnapshots.Any())
            context.AddRange(VendorSnapshotSeed.Data);

        if (!context.BrandSnapshots.Any())
            context.AddRange(BrandSnapshotSeed.Data);

        if (!context.CharacteristicSnapshots.Any())
            context.AddRange(CharacteristicSnapshotSeed.Data);

        await context.SaveChangesAsync();
    }
}