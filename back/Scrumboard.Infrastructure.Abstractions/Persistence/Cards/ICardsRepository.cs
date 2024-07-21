using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public interface ICardsRepository
{
    Task<Card?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Card> AddAsync(CardCreation cardCreation, CancellationToken cancellationToken = default);
    Task<Card> UpdateAsync(CardEdition cardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
