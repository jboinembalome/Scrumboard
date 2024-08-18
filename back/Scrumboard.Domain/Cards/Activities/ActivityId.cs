using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards.Activities;

public sealed class ActivityId(
    int value) : IntStrongId<ActivityId>(value), IStrongId<ActivityId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator ActivityId?(int? id)
        => id is null ? null : new ActivityId(id.Value);
}
