using AutoMapper;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

internal sealed class ListBoardsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IListBoardsRepository
{
    public async Task<ListBoard?> TryGetByIdAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default) 
        => await dbContext.ListBoards.FindAsync([id], cancellationToken);

    public async Task<ListBoard> AddAsync(
        ListBoardCreation listBoardCreation, 
        CancellationToken cancellationToken = default)
    {
        var listBoard = mapper.Map<ListBoard>(listBoardCreation);
        
        dbContext.ListBoards.Add(listBoard);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return listBoard;
    }

    public async Task<ListBoard> UpdateAsync(
        ListBoardEdition listBoardEdition, 
        CancellationToken cancellationToken = default)
    {
        var listBoard = await dbContext.ListBoards.FindAsync([listBoardEdition.Id], cancellationToken)
            .OrThrowEntityNotFoundAsync();

        mapper.Map(listBoardEdition, listBoard);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return listBoard;
    }

    public async Task DeleteAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default)
    {
        var listBoard = await dbContext.ListBoards.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.ListBoards.Remove(listBoard);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
