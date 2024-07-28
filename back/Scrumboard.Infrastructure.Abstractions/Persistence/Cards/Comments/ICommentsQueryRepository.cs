using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public interface ICommentsQueryRepository
{
    Task<Comment?> TryGetByIdAsync(CommentId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Comment>> GetByCardIdAsync(CardId cardId, CancellationToken cancellationToken = default);
}
