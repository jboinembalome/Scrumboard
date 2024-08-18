using System.Globalization;

namespace Scrumboard.SharedKernel.Types;

public abstract class GuidStrongId<TStrongType>(
    Guid value) : StrongId<TStrongType, Guid>(value)
    where TStrongType : StrongId<TStrongType, Guid>, IStrongId<TStrongType, Guid>
{
    public override string ToString()
        => Value.ToString("D", CultureInfo.InvariantCulture);

    public static explicit operator Guid(GuidStrongId<TStrongType> id)
        => id.Value;
}
