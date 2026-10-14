using MediatR;

namespace CartService.Application.Features.IntegrationEventHandlers.ProductSnapshot.DeleteProductSnapshot;

public sealed record DeleteProductSnapshotCommand(Guid ProductId) : IRequest;