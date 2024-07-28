using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICommentsQueryRepository
{
    public async Task<Comment?> TryGetByIdAsync(CommentId id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Comment>(dao);
    }

    public async Task<IReadOnlyList<Comment>> GetByCardIdAsync(CardId cardId, CancellationToken cancellationToken = default)
    {
        var daos = await dbContext.Comments
            .AsNoTracking()
            .Where(x => x.CardId == cardId)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Comment>>(daos);
    }
}
