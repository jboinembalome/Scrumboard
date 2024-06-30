using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public interface ICardsQueryRepository
{
    Task<Card?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
}
