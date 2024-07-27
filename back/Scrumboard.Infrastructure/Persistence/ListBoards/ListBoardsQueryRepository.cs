using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

internal sealed class ListBoardsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IListBoardsQueryRepository
{
    public async Task<ListBoard?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<ListBoard>(dao);
    }
    
    private IQueryable<ListBoardDao> Query() 
        => dbContext.ListBoards
            .AsNoTracking();
}
