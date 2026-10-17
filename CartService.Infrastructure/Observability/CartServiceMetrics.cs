using CartService.Application.Interfaces.Metrics;
using System.Diagnostics.Metrics;

namespace CartService.Infrastructure.Observability;

public sealed class CartServiceMetrics : IDisposable, ICartMetrics
{
    public const string MeterName = "CartService";
    private readonly Meter _meter;

    private readonly Counter<long> _cartsCreated;
    private readonly Counter<long> _itemPriceSynced;
    private readonly Counter<long> _cartItemsRestored;
    private readonly Counter<long> _cartItemsSuspended;

    private readonly Counter<long> _handlerErrors;
    private readonly Histogram<double> _handlerDuration;

    private readonly Counter<long> _snapshotNotFound;
    private readonly Counter<long> _snapshotOutdated;

    public CartServiceMetrics()
    {
        _meter = new Meter(MeterName);

        _cartsCreated = _meter.CreateCounter<long>(
            "cart.carts.created");

        _itemPriceSynced = _meter.CreateCounter<long>(
            "cart.cart_item.price_synced");

        _cartItemsRestored = _meter.CreateCounter<long>(
            "cart.cart_items.restored");

        _cartItemsSuspended = _meter.CreateCounter<long>(
            "cart.cart_items.suspended");

        _handlerErrors = _meter.CreateCounter<long>(
            "cart.handlers.errors");

        _handlerDuration = _meter.CreateHistogram<double>(
            "cart.handlers.duration",
            unit: "ms");

        _snapshotNotFound = _meter.CreateCounter<long>(
            "cart.snapshots.not_found",
            description: "Snapshot missing when projection arrived — possible race condition");

        _snapshotOutdated = _meter.CreateCounter<long>(
            "cart.snapshots.outdated",
            description: "Projection skipped because version is outdated");
    }

    public void RecordCartCreated() => _cartsCreated.Add(1);

    public void RecordItemPriceSynced() => _itemPriceSynced.Add(1);

    public void RecordCartItemsSuspended(string reason) =>
        _cartItemsSuspended.Add(1, new KeyValuePair<string, object?>("reason", reason));

    public void RecordCartItemsRestored() => _cartItemsRestored.Add(1);

    public void RecordHandlerError(string handlerName, string handlerType) =>
        _handlerErrors.Add(1,
            new KeyValuePair<string, object?>("handler", handlerName),
            new KeyValuePair<string, object?>("type", handlerType));

    public void RecordHandlerDuration(double ms, string handlerName, string handlerType) =>
        _handlerDuration.Record(ms,
            new KeyValuePair<string, object?>("handler", handlerName),
            new KeyValuePair<string, object?>("type", handlerType));

    public void RecordSnapshotNotFound(string snapshotType) =>
        _snapshotNotFound.Add(1, new KeyValuePair<string, object?>("type", snapshotType));

    public void RecordSnapshotOutdated(string handlerName) =>
        _snapshotOutdated.Add(1, new KeyValuePair<string, object?>("handler", handlerName));

    public void Dispose() => _meter.Dispose();
}