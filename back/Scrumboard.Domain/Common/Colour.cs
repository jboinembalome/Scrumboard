using Scrumboard.Domain.Common.Errors;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Common;

public sealed class Colour : ValueObject
{
    static Colour()
    {
    }

    private Colour()
    {
    }

    private Colour(string code) => Code = code;

    public static Colour From(string code)
    {
        var colour = new Colour { Code = code };

        if (!SupportedColours.Contains(colour))
            throw new UnsupportedColourException(code);

        return colour;
    }

    public static Colour Gray => new("bg-gray-800/30");

    public static Colour Teal => new("bg-teal-800/30");

    public static Colour Green => new("bg-green-800/30");

    public static Colour Amber => new("bg-amber-800/30");

    public static Colour Orange => new("bg-orange-800/30");

    public static Colour Violet => new("bg-violet-800/30");

    public static Colour Purple => new("bg-purple-800/30");

    public static Colour Pink => new("bg-pink-800/30");

    public static Colour Rose => new("bg-rose-800/30");

    public static Colour Indigo => new("bg-indigo-800/30");

    public static Colour Red => new("bg-red-800/30");

    public static Colour Yellow => new("bg-yellow-800/30");

    public static Colour Blue => new("bg-blue-800/30");  

    public string Code { get; private set; }

    public static implicit operator string(Colour colour) => colour.ToString();

    public static explicit operator Colour(string code) => From(code);

    public override string ToString() => Code;

    public static IEnumerable<Colour> SupportedColours
    {
        get
        {
            yield return Gray;
            yield return Teal;
            yield return Green;
            yield return Amber;
            yield return Orange;
            yield return Violet;
            yield return Purple;
            yield return Pink;
            yield return Rose;
            yield return Indigo;
            yield return Red;
            yield return Yellow;
            yield return Blue;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
