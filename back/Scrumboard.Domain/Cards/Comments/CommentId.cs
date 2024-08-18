using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards.Comments;

public sealed class CommentId(
    int value) : IntStrongId<CommentId>(value), IStrongId<CommentId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator CommentId?(int? id)
        => id is null ? null : new CommentId(id.Value);
}
