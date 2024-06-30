using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IBoardsRepository
{
    public async Task<Board?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Board>(dao);
    }

    public async Task<Board> AddAsync(Board board, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<BoardDao>(board);
        
        dbContext.Boards.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Board>(dao);
    }

    public async Task<Board> UpdateAsync(Board board, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstAsync(x => x.Id == board.Id, cancellationToken);

        mapper.Map(board, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Board>(dao);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Boards.FirstAsync(x => x.Id == id, cancellationToken);
        
        dbContext.Boards.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<BoardDao> Query()
        => dbContext.Boards
            .AsSplitQuery()
            .Include(b => b.BoardSetting)
            .Include(b => b.ListBoards)
                .ThenInclude(l => l.Cards);
}
