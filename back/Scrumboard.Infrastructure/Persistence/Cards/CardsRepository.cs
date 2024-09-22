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
    {
        var card = await dbContext.Cards.FindAsync([id], cancellationToken);

        if (card is null)
        {
            return card;
        }

        await LoadNavigationPropertiesAsync(card, cancellationToken);

        return card;
    }

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

    private async Task LoadNavigationPropertiesAsync(Card card, CancellationToken cancellationToken)
    {
        await dbContext.LoadNavigationPropertyAsync(card, x => x.Assignees, cancellationToken);
        await dbContext.LoadNavigationPropertyAsync(card, x => x.Labels, cancellationToken);
    }
}
