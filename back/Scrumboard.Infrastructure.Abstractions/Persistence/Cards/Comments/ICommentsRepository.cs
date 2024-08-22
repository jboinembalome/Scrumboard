using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public interface ICommentsRepository
{
    Task<Comment?> TryGetByIdAsync(CommentId id, CancellationToken cancellationToken = default);
    Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Comment Update(Comment comment);
    Task DeleteAsync(CommentId id, CancellationToken cancellationToken = default);
}
