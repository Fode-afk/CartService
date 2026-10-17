namespace CartService.Application.Caching;

internal static class CacheKeys
{
    public static string CartByBuyerId(Guid buyerId) =>
        $"cart:buyer-id:{buyerId}";
}
