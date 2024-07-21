using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Application.Abstractions.Cards;

public interface ICardsService
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Card> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Card> AddAsync(CardCreation cardCreation, CancellationToken cancellationToken = default);
    Task<Card> UpdateAsync(CardEdition cardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
