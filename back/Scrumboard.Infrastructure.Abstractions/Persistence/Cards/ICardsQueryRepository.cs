using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public interface ICardsQueryRepository
{
    Task<IReadOnlyList<Card>> GetByListBoardIdAsync(ListBoardId listBoardId, CancellationToken cancellationToken = default);
    Task<Card?> TryGetByIdAsync(CardId id, CancellationToken cancellationToken = default);
}
