using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Application.Abstractions.Cards;

public interface ICardsService
{
    Task<bool> ExistsAsync(CardId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Card>> GetByListBoardIdAsync(ListBoardId listBoardId, CancellationToken cancellationToken = default);
    Task<Card> GetByIdAsync(CardId id, CancellationToken cancellationToken = default);
    Task<Card> AddAsync(CardCreation cardCreation, CancellationToken cancellationToken = default);
    Task<Card> UpdateAsync(CardEdition cardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(CardId id, CancellationToken cancellationToken = default);
}
