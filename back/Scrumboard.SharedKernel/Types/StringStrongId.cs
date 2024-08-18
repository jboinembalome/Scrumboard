namespace Scrumboard.SharedKernel.Types;

public abstract class StringStrongId<TStrongType>(
    string value) : StrongId<TStrongType, string>(value)
    where TStrongType : StrongId<TStrongType, string>, IStrongId<TStrongType, string>
{
    public override string ToString()
        => Value;
    
    public static explicit operator string(StringStrongId<TStrongType> id)
        => id.Value;
}
