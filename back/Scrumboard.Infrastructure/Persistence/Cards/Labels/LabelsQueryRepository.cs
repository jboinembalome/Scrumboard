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
        var daos = await LabelQuery()
            .Where(x => x.BoardId == boardId)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Label>>(daos);
    }

    private IQueryable<LabelDao> LabelQuery() 
        => dbContext.Labels
            .AsNoTracking();
}
