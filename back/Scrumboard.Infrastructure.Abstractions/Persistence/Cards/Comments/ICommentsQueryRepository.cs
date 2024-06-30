using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public interface ICommentsQueryRepository
{
    Task<Comment?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Comment>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
}
