using CartService.Domain.Models;
using CartService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Data.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems", Schemas.CartWrite);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Quantity)
            .HasConversion(
                quantity => quantity.Value,
                value => Quantity.Create(value).Value);

        builder.OwnsMany(p => p.SuspensionReasons, s =>
        {
            s.WithOwner().HasForeignKey("ProductId");
            s.ToTable("ProductSuspensionReasons", Schemas.CartWrite);

            s.Property<int>("Id");
            s.HasKey("Id");

            s.Property(x => x.Reason)
                .HasConversion<string>()
                .HasMaxLength(100);
        });

        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.VariantId);
        builder.HasIndex(x => x.VendorId);
    }
}
