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
        var keyValues = new object[] { id };
        var dao = await dbContext.Boards.FindAsync(keyValues, cancellationToken);

        return mapper.Map<ListBoard>(dao);
    }
}
