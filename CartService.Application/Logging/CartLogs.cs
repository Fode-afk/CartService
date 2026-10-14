using Microsoft.Extensions.Logging;

namespace CartService.Application.Logging;

public static partial class CartLogs
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Warning,
        Message = "Could not suspend cart {CartId} items: {Reason}. Product may already be suspended.")]
    public static partial void CartItemsSuspendFailed(
        this ILogger logger, Guid cartId, string reason);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "Could not restore Cart {CartId} items: {Reason}. Manual review required.")]
    public static partial void CartItemsRestoreFailed(
        this ILogger logger, Guid cartId, string reason);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Error,
        Message = "Could not sync CartItem: {ItemId} price in cart {CartId}: {Error}")]
    public static partial void CartItemPriceSyncFailed(
        this ILogger logger, Guid cartId, Guid itemId, string error);
}
