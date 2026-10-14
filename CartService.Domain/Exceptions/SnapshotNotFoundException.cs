namespace CartService.Domain.Exceptions;

public sealed class SnapshotNotFoundException(string snapshotType, Guid id)
    : Exception($"{snapshotType} snapshot with ID {id} not found. " +
                $"The projection event may have arrived before the snapshot was created."), IExpectedException;
