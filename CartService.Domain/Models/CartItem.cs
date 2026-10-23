using CartService.Domain.Errors;
using CartService.Domain.ValueObjects;
using migApp.Shared.Domain.Primitives;
using migApp.Shared.Domain.ValueObjects;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Domain.Models;

public sealed class CartItem : Entity
{
    private CartItem() : base(Guid.Empty) { }
    private CartItem(
        Guid id,
        Guid variantId,
        Guid productId,
        Guid vendorId, 
        Quantity quantity,
        Money unitPrice, 
        DateTimeOffset now) : base(id)
    {
        VariantId = variantId;
        ProductId = productId;
        VendorId = vendorId;
        Quantity = quantity;
        UnitPrice = PriceWhenAdded = unitPrice;
        AddedAt = now;
    }

    public Guid VariantId { get; }
    public Guid ProductId { get; }
    public Guid VendorId { get; }

    public Quantity Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money PriceWhenAdded { get; private set; }

    private readonly HashSet<CartItemSuspensionReason> _suspensionReasons = [];
    public IReadOnlyCollection<CartItemSuspensionReason> SuspensionReasons => _suspensionReasons;
    public bool IsAvailable => _suspensionReasons.Count == 0;

    public DateTimeOffset AddedAt { get; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public Money LineTotal => UnitPrice * Quantity.Value;
    public bool IsPriceChangedSinceAdded => UnitPrice != PriceWhenAdded;

    internal static IResult<CartItem> Create(
        Guid variantId,
        Guid productId,
        Guid vendorId,
        Quantity quantity,
        Quantity availableQuantity,
        Money unitPrice,
        DateTimeOffset now)
    {
        if (quantity.Value == 0)
            return Fail<CartItem>(CartItemErrors.InvalidQuantity());

        if (quantity > availableQuantity)
            return Fail<CartItem>(CartItemErrors.MaxQuantityReached(availableQuantity));

        return Ok(
            new CartItem(
                Guid.NewGuid(),
                variantId,
                productId,
                vendorId,
                quantity,
                unitPrice,
                now));
    }

    internal IResult Increase(
        Quantity delta,
        Quantity availableQuantity,
        Money currentPrice,
        DateTimeOffset now)
    {
        if (!IsAvailable)
            return Fail(CartItemErrors.Unavailable());

        if (delta.Value == 0)
            return Fail<CartItem>(CartItemErrors.InvalidQuantity());

        if ((Quantity + delta) > availableQuantity)
            return Fail(CartItemErrors.MaxQuantityReached(availableQuantity));

        Quantity = Quantity + delta;
        PriceWhenAdded = UnitPrice;
        UnitPrice = currentPrice;
        UpdatedAt = now;

        return Ok();
    }

    internal IResult SetQuantity(
        Quantity quantity,
        Quantity availableQuantity,
        DateTimeOffset now)
    {
        if (Quantity == quantity)
            return Ok();

        if (!IsAvailable)
            return Fail(CartItemErrors.Unavailable());

        if (quantity.Value == 0)
            return Fail<CartItem>(CartItemErrors.InvalidQuantity());

        if (quantity > availableQuantity)
            return Fail(CartItemErrors.MaxQuantityReached(availableQuantity));

        Quantity = quantity;
        UpdatedAt = now;

        return Ok();
    }

    internal bool SyncPrice(Money newPrice, DateTimeOffset now)
    {
        if (UnitPrice == newPrice)
            return false;

        UnitPrice = newPrice;
        UpdatedAt = now;

        return true;
    }

    internal bool Suspend(CartItemSuspensionReason reason, DateTimeOffset now)
    {
        if (!_suspensionReasons.Add(reason))
            return false;

        UpdatedAt = now;

        return true;
    }

    internal bool TryRestore(CartItemSuspensionReason reason, DateTimeOffset now)
    {
        if (!_suspensionReasons.Remove(reason))
            return false;

        UpdatedAt = now;
        return true;
    }
}