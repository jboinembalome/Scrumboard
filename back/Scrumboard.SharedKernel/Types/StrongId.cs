using System.Globalization;

namespace Scrumboard.SharedKernel.Types;

public abstract class StrongId<TStrongType, TNativeType>(
    TNativeType value) : IEquatable<StrongId<TStrongType, TNativeType>>, IFormattable
    where TStrongType : StrongId<TStrongType, TNativeType>
    where TNativeType : notnull
{
    public TNativeType Value { get; } = value;

    public bool Equals(StrongId<TStrongType, TNativeType>? other) 
        => other is not null && Value.Equals(other.Value);

    public override bool Equals(object? obj)
    {
        if (obj is not StrongId<TStrongType, TNativeType> other)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Value.Equals(other.Value);
    }

    public static bool operator ==(StrongId<TStrongType, TNativeType>? left, StrongId<TStrongType, TNativeType>? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(StrongId<TStrongType, TNativeType>? left, StrongId<TStrongType, TNativeType>? right)
        => !(left == right);

    public override int GetHashCode()
        => Value.GetHashCode();

    public string ToString(string format)
        => ToString(format, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            format = "R";
        }

        format = format.Trim().ToUpperInvariant();
        return format switch
        {
            "L" => // log
                $"#{ToString()}",
            "R" => // regular
                ToString()!,
            _ => throw new FormatException($"The '{format}' format string is not supported.")
        };
    }
}
