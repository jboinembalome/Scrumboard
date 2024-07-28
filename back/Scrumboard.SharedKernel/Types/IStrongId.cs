namespace Scrumboard.SharedKernel.Types;

public interface IStrongId<T, TStrongId>
    where T : notnull
    where TStrongId : IStrongId<T, TStrongId>
{
    T Value { get; }

    static abstract explicit operator TStrongId(T value);
    static abstract implicit operator T(TStrongId strongId);
}
