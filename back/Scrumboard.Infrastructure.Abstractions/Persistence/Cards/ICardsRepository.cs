using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public interface ICardsRepository
{
    Task<Card?> TryGetByIdAsync(CardId id, CancellationToken cancellationToken = default);
    Task<Card> AddAsync(CardCreation cardCreation, CancellationToken cancellationToken = default);
    Task<Card> UpdateAsync(CardEdition cardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(CardId id, CancellationToken cancellationToken = default);
}
