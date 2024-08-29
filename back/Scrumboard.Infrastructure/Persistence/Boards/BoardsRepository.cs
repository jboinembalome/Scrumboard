using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardsRepository(
    ScrumboardDbContext dbContext) : IBoardsRepository
{
    public async Task<Board?> TryGetByIdAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        var board = await dbContext.Boards.FindAsync([id], cancellationToken);
        
        if (board is not null)
        {
            await dbContext.Entry(board)
                .Reference(x => x.BoardSetting)
                .LoadAsync(cancellationToken);
        }
        
        return board;
    }

    public async Task<Board> AddAsync(
        Board board, 
        CancellationToken cancellationToken = default)
    {
        await dbContext.Boards.AddAsync(board, cancellationToken);

        return board;
    }

    public Board Update(Board board)
    {
        dbContext.Boards.Update(board);
        
        return board;
    }

    public async Task DeleteAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        var board = await dbContext.Boards.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Boards.Remove(board);
    }
}
