using migApp.Shared.Domain.ValueObjects;

namespace CartService.Application.Interfaces.Jobs;

public interface ISyncCartItemPricesJob
{
    Task Execute(Guid variantId, Money unitPrice, CancellationToken cancellationToken = default);
}