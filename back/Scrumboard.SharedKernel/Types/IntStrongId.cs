using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Scrumboard.SharedKernel.Types;

public abstract class IntStrongId<TStrongType>(
    int value) : StrongId<TStrongType, int>(value)
    where TStrongType : StrongId<TStrongType, int>, IStrongId<TStrongType, int>
{
    public override string ToString()
        => Value.ToString(CultureInfo.InvariantCulture);

    [return: NotNullIfNotNull(nameof(id))]
    public static explicit operator int?(IntStrongId<TStrongType>? id)
        => id?.Value;
    
    public static explicit operator int(IntStrongId<TStrongType> id)
        => id.Value;
}
