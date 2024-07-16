using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICommentsRepository
{
    public async Task<Comment?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Comments.FindAsync(keyValues, cancellationToken);
        
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
        var keyValues = new object[] { commentEdition.Id };
        var dao = await dbContext.Comments.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);

        mapper.Map(commentEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Comment>(dao);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Comments.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);
        
        dbContext.Comments.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
