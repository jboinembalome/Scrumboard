using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Teams;

public sealed class TeamId(
    int value) : IntStrongId<TeamId>(value), IStrongId<TeamId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator TeamId?(int? id)
        => id is null ? null : new TeamId(id.Value);
}
