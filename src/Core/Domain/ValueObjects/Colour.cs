using Scrumboard.Domain.Common;
using Scrumboard.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Scrumboard.Domain.ValueObjects
{
    public class Colour : ValueObject
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

        public static Colour Black => new("bg-black");

        public static Colour White => new("bg-white");

        public static Colour Gray => new("bg-gray-500");

        public static Colour Teal => new("bg-teal-500");

        public static Colour Green => new("bg-green-500");

        public static Colour Amber => new("bg-amber-500");

        public static Colour Orange => new("bg-orange-500");

        public static Colour Violet => new("bg-violet-500");

        public static Colour Purple => new("bg-purple-500");

        public static Colour Pink => new("bg-pink-500");

        public static Colour Rose => new("bg-rose-500");

        public static Colour Indigo => new("bg-indigo-500");

        public static Colour Red => new("bg-red-500");

        public static Colour Yellow => new("bg-yellow-500");

        public static Colour Blue => new("bg-blue-500");  

        public string Code { get; private set; }

        public static implicit operator string(Colour colour) => colour.ToString();

        public static explicit operator Colour(string code) => From(code);

        public override string ToString() => Code;

        protected static IEnumerable<Colour> SupportedColours
        {
            get
            {
                yield return Black;
                yield return White;
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

}
