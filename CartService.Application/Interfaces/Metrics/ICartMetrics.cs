namespace CartService.Application.Interfaces.Metrics;

public interface ICartMetrics
{
    void RecordCartCreated();
    void RecordItemPriceSynced();
    void RecordCartItemsSuspended(string reason);
    void RecordCartItemsRestored();
    void RecordSnapshotNotFound(string snapshotType);
    void RecordSnapshotOutdated(string handlerName);
    void RecordHandlerError(string handlerName, string handlerType);
    void RecordHandlerDuration(double ms, string handlerName, string handlerType);
}