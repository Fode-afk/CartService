using CartService.Api.Grpc.V1.Protos;
using CartService.Application.Dtos;

namespace CartService.Api.Grpc.Mapping;

internal static class CartGrpcMapper
{
    public static CartResponse ToResponse(this CartDto dto)
    {
        var response = new CartResponse
        {
            CartId = dto.CartId.ToString(),
            Totals = new CartTotalsResponse
            {
                DistinctItems = dto.Totals.DistinctItems,
                TotalQuantity = dto.Totals.TotalQuantity,
                SubtotalMinor = dto.Totals.SubtotalMinor,
                Currency = dto.Totals.Currency
            }
        };

        response.Items.AddRange(dto.Items.Select(ToItemResponse));

        return response;
    }

    private static CartItemResponse ToItemResponse(CartItemDto item)
    {
        var response = new CartItemResponse
        {
            VariantId = item.VariantId.ToString(),
            ProductId = item.ProductId.ToString(),
            VendorId = item.VendorId.ToString(),
            Quantity = item.Quantity,
            UnitPriceMinor = item.UnitPriceMinor,
            LineTotalMinor = item.LineTotalMinor,
            PriceWhenAddedMinor = item.PriceWhenAddedMinor,
            IsPriceChanged = item.IsPriceChanged,
            IsAvailable = item.IsAvailable
        };

        response.SuspensionReasons.AddRange(item.SuspensionReasons);

        return response;
    }
}