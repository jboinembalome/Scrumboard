using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.Boards.ListBoards;

internal sealed class ListBoardsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IListBoardsRepository
{
    public async Task<ListBoard?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.ListBoards
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<ListBoard>(dao);
    }
}
