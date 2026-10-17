namespace CartService.Application.Dtos;

public sealed record CartTotalsDto(
    int DistinctItems,
    int TotalQuantity,
    long SubtotalMinor,
    string Currency);