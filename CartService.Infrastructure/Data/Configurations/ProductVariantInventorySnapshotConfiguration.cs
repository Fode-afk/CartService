using CartService.Domain.Snapshots;
using CartService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Data.Configurations;

internal sealed class ProductVariantInventorySnapshotConfiguration : IEntityTypeConfiguration<ProductVariantInventorySnapshot>
{
    public void Configure(EntityTypeBuilder<ProductVariantInventorySnapshot> builder)
    {
        builder.ToTable("ProductVariantInventorySnapshots", Schemas.CartWrite);

        builder.HasKey(x => x.ProductVariantId);

        builder.Property(x => x.AvailableQuantity)
            .HasConversion(
                quantity => quantity.Value,
                value => Quantity.Create(value).Value);

        builder.Property<byte[]>("RowVersion")
           .IsRowVersion()
           .IsConcurrencyToken();
    }
}
