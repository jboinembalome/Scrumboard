using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public interface ICardsQueryRepository
{
    Task<IReadOnlyList<Card>> GetByListBoardIdAsync(int listBoardId, CancellationToken cancellationToken = default);
    Task<Card?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
}
