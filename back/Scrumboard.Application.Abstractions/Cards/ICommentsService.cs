using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Abstractions.Cards;

public interface ICommentsService
{
    Task<Comment> GetByIdAsync(CommentId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Comment>> GetByCardIdAsync(CardId cardId, CancellationToken cancellationToken = default);
    Task<Comment> AddAsync(CommentCreation commentCreation, CancellationToken cancellationToken = default);
    Task<Comment> UpdateAsync(CommentEdition commentEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(CommentId id, CancellationToken cancellationToken = default);
}
