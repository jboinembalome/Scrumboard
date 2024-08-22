using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public interface ICardsRepository
{
    Task<Card?> TryGetByIdAsync(CardId id, CancellationToken cancellationToken = default);
    Task<Card> AddAsync(Card card, CancellationToken cancellationToken = default);
    Card Update(Card card);
    Task DeleteAsync(CardId id, CancellationToken cancellationToken = default);
}
