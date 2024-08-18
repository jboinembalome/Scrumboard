namespace Scrumboard.SharedKernel.Types;

public interface IStrongId<out TStrongType, in TNativeType>
    where TNativeType : notnull
    where TStrongType : IStrongId<TStrongType, TNativeType>;
