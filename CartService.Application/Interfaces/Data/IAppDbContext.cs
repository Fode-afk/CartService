using CartService.Domain.Models;
using CartService.Domain.Snapshots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CartService.Application.Interfaces.Data;

public interface IAppDbContext
{
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }

    DbSet<ProductSnapshot> ProductSnapshots { get; }
    DbSet<ProductVariantSnapshot> ProductVariantSnapshots { get; }
    DbSet<ProductVariantInventorySnapshot> ProductVariantInventorySnapshots { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    void ClearChangeTracker();
}