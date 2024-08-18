using System.Diagnostics.CodeAnalysis;

namespace Scrumboard.SharedKernel.Types;

public abstract class StringStrongId<TStrongType>(
    string value) : StrongId<TStrongType, string>(value)
    where TStrongType : StrongId<TStrongType, string>, IStrongId<TStrongType, string>
{
    public override string ToString()
        => Value;
    
    [return: NotNullIfNotNull(nameof(id))]
    public static explicit operator string?(StringStrongId<TStrongType>? id)
        => id?.Value;
}
