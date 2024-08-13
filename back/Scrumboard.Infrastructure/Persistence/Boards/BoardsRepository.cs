using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IBoardsRepository
{
    public async Task<Board?> TryGetByIdAsync(
        BoardId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Board> AddAsync(
        BoardCreation boardCreation, 
        CancellationToken cancellationToken = default)
    {
        var board = mapper.Map<Board>(boardCreation);
        
        await dbContext.Boards.AddAsync(board, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return board;
    }

    public async Task<Board> UpdateAsync(
        BoardEdition boardEdition, 
        CancellationToken cancellationToken = default)
    {
        var board = await Query()
            .FirstAsync(x => x.Id == boardEdition.Id, cancellationToken);

        mapper.Map(boardEdition, board);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return board;
    }

    public async Task DeleteAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        var board = await dbContext.Boards.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Boards.Remove(board);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Board> Query()
        => dbContext.Boards
            .Include(b => b.BoardSetting);
}
