using CartService.Domain.Errors;
using CartService.Domain.Primitives;
using CartService.Domain.ValueObjects;
using migApp.Shared.Domain.ValueObjects;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Domain.Models;

public sealed class CartItem : Entity
{
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

    internal static CartItem Create(
        Guid variantId,
        Guid productId, 
        Guid vendorId, 
        Quantity quantity,
        Money unitPrice, 
        DateTimeOffset now) =>
        new(
            Guid.NewGuid(),
            variantId, 
            productId, 
            vendorId, 
            quantity, 
            unitPrice, 
            now);

    internal IResult Increase(
        Quantity delta,
        Money currentPrice,
        DateTimeOffset now)
    {
        if (!IsAvailable)
            return Fail(CartItemErrors.Unavailable());

        Quantity = Quantity + delta;
        PriceWhenAdded = UnitPrice;
        UnitPrice = currentPrice;
        UpdatedAt = now;

        return Ok();
    }

    internal IResult SetQuantity(Quantity quantity, DateTimeOffset now)
    {
        if (Quantity == quantity)
            return Ok();

        if (!IsAvailable)
            return Fail(CartItemErrors.Unavailable());

        Quantity = quantity;
        UpdatedAt = now;

        return Ok();
    }

    internal void SyncPrice(Money newPrice, DateTimeOffset now)
    {
        if (UnitPrice == newPrice)
            return;

        UnitPrice = newPrice;
        UpdatedAt = now;
    }

    internal void Suspend(CartItemSuspensionReason reason, DateTimeOffset now)
    {
        if (!IsAvailable)
            return;

        _suspensionReasons.Add(reason);

        UpdatedAt = now;
    }

    internal void TryRestore(CartItemSuspensionReason reason, DateTimeOffset now)
    {
        if (IsAvailable)
            return;

        _suspensionReasons.Remove(reason);
            
        UpdatedAt = now;
    }
}