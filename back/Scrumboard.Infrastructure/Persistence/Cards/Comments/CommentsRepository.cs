using AutoMapper;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICommentsRepository
{
    public async Task<Comment?> TryGetByIdAsync(CommentId id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Comments.FindAsync([id], cancellationToken);
        
        return mapper.Map<Comment>(dao);
    }

    public async Task<Comment> AddAsync(CommentCreation commentCreation, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<CommentDao>(commentCreation);
        
        dbContext.Comments.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Comment>(dao);
    }

    public async Task<Comment> UpdateAsync(CommentEdition commentEdition, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Comments.FindAsync([commentEdition.Id], cancellationToken)
            .OrThrowEntityNotFoundAsync();

        mapper.Map(commentEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Comment>(dao);
    }

    public async Task DeleteAsync(CommentId id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Comments.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Comments.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
