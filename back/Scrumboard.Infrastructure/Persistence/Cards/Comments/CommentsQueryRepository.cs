using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentsQueryRepository(
    ScrumboardDbContext dbContext) : ICommentsQueryRepository
{
    public async Task<Comment?> TryGetByIdAsync(
        CommentId id, 
        CancellationToken cancellationToken = default) 
        => await dbContext.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Comment>> GetByCardIdAsync(
        CardId cardId, 
        CancellationToken cancellationToken = default) 
        => await dbContext.Comments
            .AsNoTracking()
            .Where(x => x.CardId == cardId)
            .ToListAsync(cancellationToken);
}
