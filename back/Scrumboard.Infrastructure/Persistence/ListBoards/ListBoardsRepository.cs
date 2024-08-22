using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

internal sealed class ListBoardsRepository(
    ScrumboardDbContext dbContext) : IListBoardsRepository
{
    public async Task<ListBoard?> TryGetByIdAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default) 
        => await dbContext.ListBoards.FindAsync([id], cancellationToken);

    public async Task<ListBoard> AddAsync(
        ListBoard listBoard, 
        CancellationToken cancellationToken = default)
    {
        await dbContext.ListBoards.AddAsync(listBoard, cancellationToken);

        return listBoard;
    }

    public ListBoard Update(ListBoard listBoard)
    {
        dbContext.ListBoards.Update(listBoard);
        
        return listBoard;
    }

    public async Task DeleteAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default)
    {
        var listBoard = await dbContext.ListBoards.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.ListBoards.Remove(listBoard);
    }
}
