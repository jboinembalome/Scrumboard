using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Teams;

public readonly record struct TeamId(int Value)
    : IStrongId<int, TeamId>
{
    public static implicit operator int(TeamId strongId)
        => strongId.Value;

    public static explicit operator TeamId(int value)
        => new(value);
}
