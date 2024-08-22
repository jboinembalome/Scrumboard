using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentsRepository(
    ScrumboardDbContext dbContext) : ICommentsRepository
{
    public async Task<Comment?> TryGetByIdAsync(
        CommentId id, 
        CancellationToken cancellationToken = default) 
        => await dbContext.Comments.FindAsync([id], cancellationToken);

    public async Task<Comment> AddAsync(
        Comment comment, 
        CancellationToken cancellationToken = default)
    {
        await dbContext.Comments.AddAsync(comment, cancellationToken);

        return comment;
    }

    public Comment Update(Comment comment)
    {
        dbContext.Comments.Update(comment);
        
        return comment;
    }

    public async Task DeleteAsync(
        CommentId id, 
        CancellationToken cancellationToken = default)
    {
        var comment = await dbContext.Comments.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Comments.Remove(comment);
    }
}
