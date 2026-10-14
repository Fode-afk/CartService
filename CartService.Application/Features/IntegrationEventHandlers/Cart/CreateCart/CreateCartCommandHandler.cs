using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Metrics;
using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.Cart.CreateCart;

public sealed class CreateCartCommandHandler(
    IAppDbContext context,
    ICartMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<CreateCartCommand>
{
    public async Task Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = Domain.Models.Cart.Create(request.BuyerId, timeProvider.GetUtcNow());

        context.Carts.Add(cart);

        await context.SaveChangesAsync(cancellationToken);

        metrics.RecordCartCreated();
    }
}
