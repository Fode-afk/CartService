using CartService.Application.Interfaces.Data;
using CartService.Domain.Models;
using CartService.Domain.Snapshots;
using CartService.Infrastructure.DomainEvents;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using migApp.Shared.Domain.Primitives;

namespace CartService.Infrastructure.Data;

internal sealed class AppDbContext(
    DbContextOptions<AppDbContext> opt,
    IDomainEventsDispatcher domainEventsDispatcher) : DbContext(opt), IAppDbContext
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<ProductSnapshot> ProductSnapshots { get; set; }
    public DbSet<ProductVariantSnapshot> ProductVariantSnapshots { get; set; }
    public DbSet<ProductVariantInventorySnapshot> ProductVariantInventorySnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schemas.CartWrite);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        modelBuilder.Entity<OutboxMessage>()
            .ToTable("OutboxMessages", Schemas.Messaging);

        modelBuilder.Entity<OutboxState>()
            .ToTable("OutboxState", Schemas.Messaging);

        modelBuilder.Entity<InboxState>()
            .ToTable("InboxState", Schemas.Messaging);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishPreCommitDomainEventsEventsAsync(cancellationToken);

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishPostCommitDomainEventsAsync(cancellationToken);

        ClearDomainEvents();

        return result;
    }

    private async Task PublishPreCommitDomainEventsEventsAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        await domainEventsDispatcher.DispatchPreCommitDomainEventsAsync(domainEvents, cancellationToken);
    }

    private async Task PublishPostCommitDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        var localEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        await domainEventsDispatcher.DispatchPostCommitDomainEventsAsync(localEvents, cancellationToken);
    }

    private void ClearDomainEvents()
    {
        foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
            entry.Entity.ClearDomainEvents();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        Database.BeginTransactionAsync(cancellationToken);

    public void ClearChangeTracker() => ChangeTracker.Clear();
}