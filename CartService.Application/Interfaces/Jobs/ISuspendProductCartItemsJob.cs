namespace CartService.Application.Interfaces.Jobs;

public interface ISuspendProductCartItemsJob
{
    Task Execute(Guid productId, bool isVisiblePublicly, CancellationToken cancellationToken = default);
}