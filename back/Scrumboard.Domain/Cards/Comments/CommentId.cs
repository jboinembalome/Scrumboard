using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards.Comments;

public readonly record struct CommentId(int Value)
    : IStrongId<int, CommentId>
{
    public static implicit operator int(CommentId strongId)
        => strongId.Value;

    public static explicit operator CommentId(int value)
        => new(value);
}
