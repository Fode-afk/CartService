using CartService.Domain.DomainEvents;
using CartService.Domain.Errors;
using CartService.Domain.Primitives;
using CartService.Domain.ValueObjects;
using migApp.Shared.Domain.ValueObjects;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Domain.Models;

public sealed class Cart : AggregateRoot
{
    private Cart(
        Guid id,
        Guid buyerId,
        DateTimeOffset now) : base(id)
    {
        BuyerId = buyerId;
        CreatedAt = now;
    }

    public Guid BuyerId { get; }

    public const int MaxDistinctItems = 100;
    private readonly List<CartItem> _items = [];
    public IReadOnlyCollection<CartItem> Items => _items;

    public bool IsEmpty => _items.Count == 0;

    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public Money Subtotal =>
        _items
            .Where(i => i.IsAvailable)
            .Aggregate(Money.Zero(Currency.USD), (sum, i) => sum + i.LineTotal);

    public static Cart Create(
        Guid buyerId,
        DateTimeOffset now)
    {
        var cart = new Cart(
            Guid.NewGuid(),
            buyerId, 
            now);

        cart.RaiseDomainEvent(new CartCreatedDomainEvent(cart.Id));

        return cart;
    }

    public IResult<CartItem?> AddItem(
        Guid variantId,
        Guid productId, 
        Guid vendorId, 
        Quantity quantity, 
        Money unitPrice, DateTimeOffset now)
    {
        var existing = _items.FirstOrDefault(i => i.VariantId == variantId);

        CartItem? cartItem = null;
        if (existing is null)
        {
            if (_items.Count >= MaxDistinctItems)
                return Fail<CartItem>(CartErrors.DistinctItemsLimitReached(MaxDistinctItems));

            var item = CartItem.Create(
                variantId,
                productId,
                vendorId,
                quantity,
                unitPrice,
                now);

            _items.Add(item);
            cartItem = item;
        }
        else
        {
            var result = existing.Increase(quantity, unitPrice, now);

            if (result.IsFailure)
                return Fail<CartItem>(result.Error);
        }

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartItemAddedDomainEvent(
            Id,
            Version));

        return Ok(cartItem);
    }

    public IResult ChangeQuantity(
        Guid variantId,
        Quantity quantity, 
        DateTimeOffset now)
    {
        var item = _items.FirstOrDefault(i => i.VariantId == variantId);

        if (item is null) 
            return Fail(CartItemErrors.NotFound());

        var result = item.SetQuantity(quantity, now);

        if (result.IsFailure) 
            return result;

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartItemQuantityChangedDomainEvent(
            Id, 
            Version));

        return Ok();
    }

    public IResult RemoveItem(Guid variantId, DateTimeOffset now)
    {
        var item = _items.FirstOrDefault(i => i.VariantId == variantId);

        if (item is null) 
            return Fail(CartItemErrors.NotFound());

        _items.Remove(item);

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartItemRemovedDomainEvent(
            Id, 
            Version));

        return Ok();
    }

    public IResult Clear(DateTimeOffset now)
    {
        if (IsEmpty) 
            return Ok();

        _items.Clear();

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartClearedDomainEvent(
            Id, 
            Version));

        return Ok();
    }

    public IResult RemoveUnavailable(DateTimeOffset now)
    {
        var removed = _items.RemoveAll(i => !i.IsAvailable);

        if (removed == 0)
            return Ok();

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartItemsRemovedUnavailableDomainEvent(
            Id,
            Version));

        return Ok();
    }

    public IResult SyncItemPrice(
        Guid variantId,
        Money newPrice, 
        DateTimeOffset now)
    {
        var item = _items.FirstOrDefault(i => i.VariantId == variantId);

        if (item is null)
            return Fail(CartItemErrors.NotFound());

        var old = item.UnitPrice;

        item.SyncPrice(newPrice, now);

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartItemPriceChangedDomainEvent(
            Id,
            Version));

        return Ok();
    }
    public IResult SuspendItems(
        Guid productId,
        CartItemSuspensionReason reason,
        DateTimeOffset now)
    {
        var items = _items.Where(i => i.ProductId == productId).ToList();

        if (items.Count == 0)
            return Fail(CartItemErrors.NotFound());

        foreach (var item in items)
            item.Suspend(reason, now);

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartItemsSuspendedDomainEvent(
            Id,
            Version));

        return Ok();
    }

    public IResult TryRestoreItems(
        Guid productId,
        CartItemSuspensionReason reason,
        DateTimeOffset now)
    {
        var items = _items.Where(i => i.ProductId == productId).ToList();

        if (items.Count == 0)
            return Fail(CartItemErrors.NotFound());

        foreach (var item in items)
            item.TryRestore(reason, now);

        UpdatedAt = now;

        IncreaseVersion();

        RaiseDomainEvent(new CartItemsRestoredDomainEvent(
            Id,
            Version));

        return Ok();
    }
}