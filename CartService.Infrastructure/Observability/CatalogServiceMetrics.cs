using CartService.Application.Interfaces.Metrics;
using System.Diagnostics.Metrics;

namespace CartService.Infrastructure.Observability;

public sealed class CatalogServiceMetrics : IDisposable, ICartMetrics
{
    public const string MeterName = "CatalogService";
    private readonly Meter _meter;

    private readonly Counter<long> _productsCreated;
    private readonly Counter<long> _productsSuspended;
    private readonly Counter<long> _productsRestored;
    private readonly Counter<long> _productsDeleted;
    private readonly Counter<long> _productNotFound;
    private int _activeProductsCount_value;
    private readonly ObservableGauge<int> _activeProductsCount;

    private readonly Counter<long> _handlerErrors;
    private readonly Histogram<double> _handlerDuration;

    private readonly Counter<long> _snapshotNotFound;
    private readonly Counter<long> _snapshotOutdated;

    public CatalogServiceMetrics()
    {
        _meter = new Meter(MeterName);

        _productsCreated = _meter.CreateCounter<long>(
            "catalog.products.created");

        _productsSuspended = _meter.CreateCounter<long>(
            "catalog.products.suspended");

        _productsRestored = _meter.CreateCounter<long>(
            "catalog.products.restored");

        _productsDeleted = _meter.CreateCounter<long>(
            "catalog.products.deleted");

        _productNotFound = _meter.CreateCounter<long>(
            "catalog.products.not_found");

        _activeProductsCount = _meter.CreateObservableGauge(
            "catalog.products.active",
            () => _activeProductsCount_value);

        _handlerErrors = _meter.CreateCounter<long>(
            "catalog.handlers.errors");

        _handlerDuration = _meter.CreateHistogram<double>(
            "catalog.handlers.duration",
            unit: "ms");

        _snapshotNotFound = _meter.CreateCounter<long>(
            "catalog.snapshots.not_found",
            description: "Snapshot missing when projection arrived — possible race condition");

        _snapshotOutdated = _meter.CreateCounter<long>(
            "catalog.snapshots.outdated",
            description: "Projection skipped because version is outdated");
    }

    public void RecordCartCreated() => _productsCreated.Add(1);

    public void RecordProductSuspended(string reason) =>
        _productsSuspended.Add(1, new KeyValuePair<string, object?>("reason", reason));

    public void RecordProductRestored() => _productsRestored.Add(1);

    public void RecordProductDeleted() => _productsDeleted.Add(1);

    public void RecordCartNotFound() => _productNotFound.Add(1);

    public void SetActiveProductsCount(int count) =>
        _activeProductsCount_value = count;

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