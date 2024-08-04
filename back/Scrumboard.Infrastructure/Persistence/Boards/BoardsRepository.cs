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
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Board>(dao);
    }

    public async Task<Board> AddAsync(
        BoardCreation boardCreation, 
        CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<BoardDao>(boardCreation);
        
        dbContext.Boards.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Board>(dao);
    }

    public async Task<Board> UpdateAsync(
        BoardEdition boardEdition, 
        CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstAsync(x => x.Id == boardEdition.Id, cancellationToken);

        mapper.Map(boardEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Board>(dao);
    }

    public async Task DeleteAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Boards.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
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
