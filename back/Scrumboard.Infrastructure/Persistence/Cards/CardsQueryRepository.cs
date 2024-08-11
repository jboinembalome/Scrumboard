using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardsQueryRepository(
    ScrumboardDbContext dbContext) : ICardsQueryRepository
{
    public async Task<IReadOnlyList<Card>> GetByListBoardIdAsync(
        ListBoardId listBoardId, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .Where(x => x.ListBoardId == listBoardId)
            .ToListAsync(cancellationToken);

    public async Task<Card?> TryGetByIdAsync(
        CardId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    private IQueryable<Card> Query()
        => dbContext.Cards
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Labels)
            .Include(x => x.Assignees);
}
