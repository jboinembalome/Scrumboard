using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards.Labels;

public sealed class LabelId(
    int value) : IntStrongId<LabelId>(value), IStrongId<LabelId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator LabelId?(int? id)
        => id is null ? null : new LabelId(id.Value);
}
