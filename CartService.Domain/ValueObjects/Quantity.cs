using CartService.Domain.Errors;
using migApp.Shared.Domain.Primitives;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Domain.ValueObjects;

public sealed class Quantity : ValueObject
{
    public int Value { get; }
    private Quantity(int value) =>
        Value = value;

    public static IResult<Quantity> Create(int value) =>
        value < 0 ? 
            Fail<Quantity>(QuantityErrors.Negative()) :
            Ok(new Quantity(value));

    public static Quantity operator +(Quantity left, Quantity right)
    {
        var result = Create(left.Value + right.Value);

        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);

        return result.Value;
    }

    public static Quantity operator -(Quantity left, Quantity right)
    {
        var result = Create(left.Value - right.Value);

        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);

        return result.Value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}