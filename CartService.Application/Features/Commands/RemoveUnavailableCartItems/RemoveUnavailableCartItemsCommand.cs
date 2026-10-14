using MediatR;
using migApp.Shared.Results;

namespace CartService.Application.Features.Commands.RemoveUnavailableCartItems;

public sealed record RemoveUnavailableCartItemsCommand(Guid BuyerId) : IRequest<IResult>;