using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ICardsRepository
{
    public async Task<Card?> TryGetByIdAsync(
        CardId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Card> AddAsync(
        CardCreation cardCreation, 
        CancellationToken cancellationToken = default)
    {
        var card = mapper.Map<Card>(cardCreation);
        
        await dbContext.Cards.AddAsync(card, cancellationToken);

        return card;
    }

    public async Task<Card> UpdateAsync(
        CardEdition cardEdition, 
        CancellationToken cancellationToken = default)
    {
        // TODO: Use LoadAsync or ChangeTracker
        var card = await Query()
            .FirstAsync(x => x.Id == cardEdition.Id, cancellationToken);

        mapper.Map(cardEdition, card);
        
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
