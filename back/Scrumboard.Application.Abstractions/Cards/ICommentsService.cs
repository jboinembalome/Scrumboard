using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Abstractions.Cards;

public interface ICommentsService
{
    Task<Comment> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Comment>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
    Task<Comment> AddAsync(CommentCreation commentCreation, CancellationToken cancellationToken = default);
    Task<Comment> UpdateAsync(CommentEdition commentEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
