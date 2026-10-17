using CartService.Infrastructure.Data.Seeds;

namespace CartService.Infrastructure.Data;

internal static class DbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.ProductSnapshots.Any())
            context.AddRange(ProductSnapshotSeed.Data);

        if (!context.ProductVariantSnapshots.Any())
            context.AddRange(ProductVariantSnapshotSeed.Data);

        if (!context.ProductVariantPriceSnapshots.Any())
            context.AddRange(ProductVariantPriceSnapshotSeed.Data);

        if (!context.ProductVariantInventorySnapshots.Any())
            context.AddRange(ProductVariantInventorySnapshotSeed.Data);

        await context.SaveChangesAsync();
    }
}