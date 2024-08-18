using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards.Comments;

public sealed class CommentId(
    int value) : IntStrongId<CommentId>(value), IStrongId<CommentId, int>
{
    public static explicit operator CommentId(int value)
        => new(value);
}
