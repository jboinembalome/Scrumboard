namespace Scrumboard.Application.Abstractions.Boards;

public interface IBoardsService
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
