using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Persistence.Boards;

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

internal sealed class LabelsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ILabelsQueryRepository
{
    public async Task<IReadOnlyList<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default)
    {
        var daos = await BoardQuery()
            .AsNoTracking()
            .Where(x => x.Id == boardId 
                        && x.ListBoards
                            .Any(y => y.Cards
                                .Any(z => z.Labels.Count > 0)))
            .SelectMany(x => x.ListBoards
                .SelectMany(y => y.Cards
                    .SelectMany(z => z.Labels)))
            .Distinct()
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Label>>(daos);
    }

    private IQueryable<BoardDao> BoardQuery() 
        => dbContext.Boards
            .AsNoTracking()
            .Include(x => x.ListBoards)
                .ThenInclude(y => y.Cards)
                .ThenInclude(z => z.Labels);
}
