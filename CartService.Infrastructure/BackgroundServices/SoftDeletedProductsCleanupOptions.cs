namespace CartService.Infrastructure.BackgroundServices;

public sealed class SoftDeletedProductsCleanupOptions
{
    public const string SectionName = "SoftDeletedProductsCleanup";

    public TimeSpan Interval { get; init; } = TimeSpan.FromHours(6);
    public TimeSpan RetentionPeriod { get; init; } = TimeSpan.FromDays(30);
    public int BatchSize { get; init; } = 500;
}
