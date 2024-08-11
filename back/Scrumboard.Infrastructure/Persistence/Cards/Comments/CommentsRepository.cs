using AutoMapper;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICommentsRepository
{
    public async Task<Comment?> TryGetByIdAsync(
        CommentId id, 
        CancellationToken cancellationToken = default) 
        => await dbContext.Comments.FindAsync([id], cancellationToken);

    public async Task<Comment> AddAsync(
        CommentCreation commentCreation, 
        CancellationToken cancellationToken = default)
    {
        var comment = mapper.Map<Comment>(commentCreation);
        
        dbContext.Comments.Add(comment);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return comment;
    }

    public async Task<Comment> UpdateAsync(
        CommentEdition commentEdition, 
        CancellationToken cancellationToken = default)
    {
        var comment = await dbContext.Comments.FindAsync([commentEdition.Id], cancellationToken)
            .OrThrowEntityNotFoundAsync();

        mapper.Map(commentEdition, comment);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return comment;
    }

    public async Task DeleteAsync(
        CommentId id, 
        CancellationToken cancellationToken = default)
    {
        var comment = await dbContext.Comments.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Comments.Remove(comment);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
