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


        builder.ComplexProperty(e => e.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(p => p.Currency)
                .HasMaxLength(Currency.CodeLength)
                .HasConversion(
                    currency => currency.Code,
                    value => Currency.Create(value).Value);
        });

        builder.Property<byte[]>("RowVersion")
           .IsRowVersion()
           .IsConcurrencyToken();
    }
}