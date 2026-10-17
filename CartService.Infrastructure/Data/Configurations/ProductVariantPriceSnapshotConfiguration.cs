using CartService.Domain.Snapshots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using migApp.Shared.Domain.ValueObjects;

namespace CartService.Infrastructure.Data.Configurations;

internal sealed class ProductVariantPriceSnapshotConfiguration : IEntityTypeConfiguration<ProductVariantPriceSnapshot>
{
    public void Configure(EntityTypeBuilder<ProductVariantPriceSnapshot> builder)
    {
        builder.ToTable("ProductVariantPriceSnapshots", Schemas.CartWrite);

        builder.HasKey(x => x.ProductVariantId);

        builder.HasIndex(x => x.ProductId);

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 4)
            .HasConversion(
                amount => amount.Amount,
                value => Money.Create(value, Currency.USD).Value);

        builder.Property<byte[]>("RowVersion")
           .IsRowVersion()
           .IsConcurrencyToken();
    }
}