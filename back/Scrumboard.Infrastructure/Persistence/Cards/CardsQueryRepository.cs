using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICardsQueryRepository
{
    public async Task<IReadOnlyList<Card>> GetByListBoardIdAsync(
        ListBoardId listBoardId, 
        CancellationToken cancellationToken = default)
    {
        var daos = await Query()
            .Where(x => x.ListBoardId == listBoardId)
            .ToListAsync(cancellationToken);
        
        return mapper.Map<IReadOnlyList<Card>>(daos);
    }

    public async Task<Card?> TryGetByIdAsync(
        CardId id, 
        CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Card>(dao);
    }

    private IQueryable<CardDao> Query()
        => dbContext.Cards
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Labels)
            .Include(x => x.Assignees);
}
