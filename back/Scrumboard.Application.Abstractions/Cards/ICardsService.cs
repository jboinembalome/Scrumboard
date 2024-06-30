namespace Scrumboard.Application.Abstractions.Cards;

public interface ICardsService
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
