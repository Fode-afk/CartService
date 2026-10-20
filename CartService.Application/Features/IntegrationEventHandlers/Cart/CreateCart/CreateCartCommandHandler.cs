using CartService.Application.Interfaces.Data;
using CartService.Application.Interfaces.Metrics;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.Cart.CreateCart;

public sealed class CreateCartCommandHandler(
    IAppDbContext context,
    ICartMetrics metrics,
    TimeProvider timeProvider) : IRequestHandler<CreateCartCommand>
{
    public async Task Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.Carts.AnyAsync(c => c.BuyerId == request.BuyerId, cancellationToken);

        if (exists) 
            return;

        var cart = Domain.Models.Cart.Create(request.BuyerId, timeProvider.GetUtcNow());

        context.Carts.Add(cart);

        await context.SaveChangesAsync(cancellationToken);

        metrics.RecordCartCreated();
    }
}
