using CartService.Application.Dtos;
using MediatR;
using migApp.Shared.Results;

namespace CartService.Application.Features.Queries.GetCart;

public sealed record GetCartQuery(Guid BuyerId, string Currency) : IRequest<IResult<CartDto>>;