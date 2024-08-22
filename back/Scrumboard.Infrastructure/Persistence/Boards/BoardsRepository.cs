using Microsoft.EntityFrameworkCore;
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
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

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

    private IQueryable<Board> Query()
        => dbContext.Boards
            .Include(b => b.BoardSetting);
}
