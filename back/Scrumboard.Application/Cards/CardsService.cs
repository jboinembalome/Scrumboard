using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Application.Cards;

internal sealed class CardsService(
    ICardsRepository cardsRepository) : ICardsService
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var card = await cardsRepository.TryGetByIdAsync(id, cancellationToken);

        return card is not null;
    }
}
