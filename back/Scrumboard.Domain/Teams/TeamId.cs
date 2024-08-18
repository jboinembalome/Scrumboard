using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Teams;

public sealed class TeamId(
    int value) : IntStrongId<TeamId>(value), IStrongId<TeamId, int>
{
    public static explicit operator TeamId(int value)
        => new(value);
}
