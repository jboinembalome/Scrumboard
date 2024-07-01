using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards;

internal sealed class BoardsService(
    IBoardsRepository boardsRepository) : IBoardsService
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var board = await boardsRepository.TryGetByIdAsync(id, cancellationToken);

        return board is not null;
    }
}
