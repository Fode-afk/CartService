using MediatR;
using migApp.Shared.Results;

namespace CartService.Application.Features.Commands.ClearCart;

public sealed record ClearCartCommand(Guid BuyerId) : IRequest<IResult>;