using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICardsRepository
{
    public async Task<Card?> TryGetByIdAsync(CardId id, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Card>(dao);
    }

    public async Task<Card> AddAsync(CardCreation cardCreation, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<CardDao>(cardCreation);
        
        dbContext.Cards.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Card>(dao);
    }

    public async Task<Card> UpdateAsync(CardEdition cardEdition, CancellationToken cancellationToken = default)
    {
        // TODO: Use LoadAsync
        var dao = await Query()
            .FirstAsync(x => x.Id == cardEdition.Id, cancellationToken);

        mapper.Map(cardEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Card>(dao);
    }

    public async Task DeleteAsync(CardId id, CancellationToken cancellationToken = default)
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
            .Include(x => x.Assignees);
}
