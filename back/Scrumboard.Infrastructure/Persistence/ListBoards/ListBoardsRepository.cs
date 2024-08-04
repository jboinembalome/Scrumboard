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
    {
        var dao = await dbContext.ListBoards.FindAsync([id], cancellationToken);

        return mapper.Map<ListBoard>(dao);
    }

    public async Task<ListBoard> AddAsync(
        ListBoardCreation listBoardCreation, 
        CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<ListBoardDao>(listBoardCreation);
        
        dbContext.ListBoards.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<ListBoard>(dao);
    }

    public async Task<ListBoard> UpdateAsync(
        ListBoardEdition listBoardEdition, 
        CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.ListBoards.FindAsync([listBoardEdition.Id], cancellationToken)
            .OrThrowEntityNotFoundAsync();

        mapper.Map(listBoardEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<ListBoard>(dao);
    }

    public async Task DeleteAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.ListBoards.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.ListBoards.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
