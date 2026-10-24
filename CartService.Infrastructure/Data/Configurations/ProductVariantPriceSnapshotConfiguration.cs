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

        builder.HasKey(x => x.PriceId);

        builder.HasIndex(x => new { x.ProductVariantId, x.Currency }).IsUnique();
        builder.HasIndex(x => x.ProductVariantId)
            .IsUnique()
            .HasFilter("[IsBase] = 1");

        builder.Property(x => x.Currency)
            .HasMaxLength(Currency.CodeLength)
            .IsRequired()
            .HasConversion(
                currency => currency.Code,
                value => Currency.Create(value).Value);

        builder.Property(x => x.AmountMinor).IsRequired();

        builder.Property<byte[]>("RowVersion")
           .IsRowVersion()
           .IsConcurrencyToken();
    }
}