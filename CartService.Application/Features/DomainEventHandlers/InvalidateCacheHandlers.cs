using CartService.Application.Caching;
using CartService.Domain.DomainEvents;
using migApp.Shared.Domain.Primitives;
using ZiggyCreatures.Caching.Fusion;

namespace CartService.Application.Features.DomainEventHandlers;

public sealed class InvalidateCacheHandlers(IFusionCache cache) :
    IPostCommitDomainEventHandler<CartItemAddedDomainEvent>,
    IPostCommitDomainEventHandler<CartItemQuantityChangedDomainEvent>,
    IPostCommitDomainEventHandler<CartItemRemovedDomainEvent>,
    IPostCommitDomainEventHandler<CartClearedDomainEvent>,
    IPostCommitDomainEventHandler<CartItemsRemovedUnavailableDomainEvent>,
    IPostCommitDomainEventHandler<CartItemPriceChangedDomainEvent>,
    IPostCommitDomainEventHandler<CartItemSuspendedDomainEvent>,
    IPostCommitDomainEventHandler<CartItemsSuspendedDomainEvent>,
    IPostCommitDomainEventHandler<CartItemRestoredDomainEvent>,
    IPostCommitDomainEventHandler<CartItemsRestoredDomainEvent>
{
    public async Task Handle(CartItemAddedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemQuantityChangedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemRemovedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartClearedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemsRemovedUnavailableDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemPriceChangedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemSuspendedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemsSuspendedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemRestoredDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);

    public async Task Handle(CartItemsRestoredDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.CartByBuyerId(notification.BuyerId), token: cancellationToken);
}
