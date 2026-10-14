namespace CartService.Domain.Exceptions;

public sealed class CartNotFoundException(Guid cartId)
    : Exception($"Cart not found: {cartId}"), IExpectedException;