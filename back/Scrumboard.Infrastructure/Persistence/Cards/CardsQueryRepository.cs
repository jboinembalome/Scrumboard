using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICardsQueryRepository
{
    public async Task<Card?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
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
            .Include(x => x.Assignees)
            .Include(x => x.Checklists)
                .ThenInclude(y => y.ChecklistItems);
}
