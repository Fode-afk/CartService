using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.UpdateProductSnapshot;

public sealed record UpdateProductSnapshotCommand(
    Guid ProductId,
    bool IsVisiblePublicly,
    long Version) : IRequest;