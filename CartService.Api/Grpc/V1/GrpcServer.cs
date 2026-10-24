using CartService.Api.Grpc.Mapping;
using CartService.Api.Grpc.V1.Protos;
using CartService.Application.Features.Commands.AddCartItem;
using CartService.Application.Features.Commands.ChangeCartItemQuantity;
using CartService.Application.Features.Commands.ClearCart;
using CartService.Application.Features.Commands.RemoveCartItem;
using CartService.Application.Features.Commands.RemoveUnavailableCartItems;
using CartService.Application.Features.Queries.GetCart;
using Grpc.Core;
using MediatR;
using migApp.Shared.Grpc;

namespace CartService.Api.Grpc.V1;

internal sealed class GrpcServer(IMediator mediator) : Protos.CartService.CartServiceBase
{
    public override async Task<CartResponse> AddCartItem(AddCartItemRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new AddCartItemCommand(
            Guid.Parse(request.BuyerId),
            Guid.Parse(request.VariantId),
            request.Quantity,
            request.CurrencyCode), context.CancellationToken);

        result.ThrowIfFailure();

        return await GetCart(request.BuyerId, request.CurrencyCode, context.CancellationToken);
    }

    public override async Task<CartResponse> ChangeCartItemQuantity(ChangeCartItemQuantityRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new ChangeCartItemQuantityCommand(
           Guid.Parse(request.BuyerId),
           Guid.Parse(request.VariantId),
           request.Quantity), context.CancellationToken);

        result.ThrowIfFailure();

        return await GetCart(request.BuyerId, request.CurrencyCode, context.CancellationToken);
    }

    public override async Task<CartResponse> ClearCart(ClearCartRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(
            new ClearCartCommand(Guid.Parse(request.BuyerId)),
            context.CancellationToken);

        result.ThrowIfFailure();

        return await GetCart(request.BuyerId, request.CurrencyCode, context.CancellationToken);
    }

    public override async Task<CartResponse> RemoveCartItem(RemoveCartItemRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new RemoveCartItemCommand(
          Guid.Parse(request.BuyerId),
          Guid.Parse(request.VariantId)), context.CancellationToken);

        result.ThrowIfFailure();

        return await GetCart(request.BuyerId, request.CurrencyCode, context.CancellationToken);
    }

    public override async Task<CartResponse> RemoveUnavailableCartItems(RemoveUnavailableCartItemsRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(
            new RemoveUnavailableCartItemsCommand(Guid.Parse(request.BuyerId)),
            context.CancellationToken);

        result.ThrowIfFailure();

        return await GetCart(request.BuyerId, request.CurrencyCode, context.CancellationToken);
    }

    public override async Task<CartResponse> GetCart(GetCartRequest request, ServerCallContext context) =>
        await GetCart(request.BuyerId, request.CurrencyCode, context.CancellationToken);

    private async Task<CartResponse> GetCart(
        string buyerId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var cart = await mediator.Send(new GetCartQuery(
                Guid.Parse(buyerId),
                currencyCode), cancellationToken);

        return cart.ThrowIfFailure().ToResponse();
    }
}