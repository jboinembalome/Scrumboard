using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICardsRepository
{
    public async Task<Card?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Card>(dao);
    }

    public async Task<Card> AddAsync(Card card, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<CardDao>(card);
        
        dbContext.Cards.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Card>(dao);
    }

    public async Task<Card> UpdateAsync(Card card, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstAsync(x => x.Id == card.Id, cancellationToken);

        mapper.Map(card, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Card>(dao);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Cards.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);
        
        dbContext.Cards.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private IQueryable<CardDao> Query()
        => dbContext.Cards
            .AsSplitQuery()
            .Include(x => x.Labels)
            .Include(x => x.Assignees)
            .Include(x => x.Checklists)
                .ThenInclude(y => y.ChecklistItems);
}
