using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardsRepository(
    ScrumboardDbContext dbContext) : ICardsRepository
{
    public async Task<Card?> TryGetByIdAsync(
        CardId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Card> AddAsync(
        Card card, 
        CancellationToken cancellationToken = default)
    {
        await dbContext.Cards.AddAsync(card, cancellationToken);

        return card;
    }

    public Card Update(Card card)
    {
        dbContext.Cards.Update(card);
        
        return card;
    }

    public async Task DeleteAsync(CardId id, CancellationToken cancellationToken = default)
    {
        var card = await dbContext.Cards.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Cards.Remove(card);
    }

    private IQueryable<Card> Query()
        => dbContext.Cards
            .AsSplitQuery()
            .Include(x => x.Labels)
            .Include(x => x.Assignees);
}
