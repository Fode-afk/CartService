using CartService.Api.Grpc.V1.Protos;
using CartService.Application.Dtos;

namespace CartService.Api.Grpc.Mapping;

internal static class CartGrpcMapper
{
    public static CartResponse ToResponse(this CartDto dto) =>
        new()
        {
            CartId = dto.CartId.ToString(),
            Items = { dto.Items.Select(ToItemResponse) }
        };

    private static CartItemResponse ToItemResponse(CartItemDto item) =>
        new()
        {
            VariantId = item.VariantId.ToString(),
            ProductId = item.ProductId.ToString(),
            VendorId = item.VendorId.ToString(),
            Quantity = item.Quantity,
            IsAvailable = item.IsAvailable,
            SuspensionReasons = { item.SuspensionReasons }
        };
}