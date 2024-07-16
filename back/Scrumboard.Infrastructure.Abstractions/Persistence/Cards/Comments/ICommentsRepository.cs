using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public interface ICommentsRepository
{
    Task<Comment?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Comment> AddAsync(CommentCreation commentCreation, CancellationToken cancellationToken = default);
    Task<Comment> UpdateAsync(CommentEdition commentEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
