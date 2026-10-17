namespace CartService.Application.Interfaces.Jobs;

public interface ISuspendInventoryCartItemJob
{
    Task Execute(Guid variantId, bool inStock, CancellationToken cancellationToken = default);
}