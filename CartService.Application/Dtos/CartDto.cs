namespace CartService.Application.Dtos;

public sealed record CartDto(
    Guid CartId,
    IReadOnlyList<CartItemDto> Items,
    CartTotalsDto Totals);