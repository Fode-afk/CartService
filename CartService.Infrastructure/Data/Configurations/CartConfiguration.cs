using CartService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Data.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts", Schemas.CartWrite);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.BuyerId).IsUnique();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey("CartId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(c => c.RowVersion)
          .IsRowVersion()
          .IsConcurrencyToken();
    }
}
