using CartService.Domain.Errors;
using migApp.Shared.Domain.Primitives;
using migApp.Shared.Results;
using static migApp.Shared.Results.ResultFactory;

namespace CartService.Domain.ValueObjects;

public sealed class Quantity : ValueObject, IComparable<Quantity>
{
    public int Value { get; }
    private Quantity(int value) =>
        Value = value;

    public static IResult<Quantity> Create(int value) =>
        value < 0 ? 
            Fail<Quantity>(QuantityErrors.Negative()) :
            Ok(new Quantity(value));

    public static Quantity operator +(Quantity left, Quantity right) => CreateOrThrow(left.Value + right.Value);
    public static Quantity operator -(Quantity left, Quantity right) => CreateOrThrow(left.Value - right.Value);

    public static bool operator >(Quantity left, Quantity right) => left.CompareTo(right) > 0;
    public static bool operator <(Quantity left, Quantity right) => left.CompareTo(right) < 0;
    public static bool operator >=(Quantity left, Quantity right) => left.CompareTo(right) >= 0;
    public static bool operator <=(Quantity left, Quantity right) => left.CompareTo(right) <= 0;

    public int CompareTo(Quantity? other)
    {
        if (other is null)
            return 1;

        return Value.CompareTo(other.Value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    private static Quantity CreateOrThrow(int value)
    {
        var result = Create(value);
        return result.IsSuccess
            ? result.Value
            : throw new InvalidOperationException(result.Error.Code);
    }
}