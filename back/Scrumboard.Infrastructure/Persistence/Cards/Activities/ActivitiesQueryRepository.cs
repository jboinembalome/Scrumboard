using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivitiesQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IActivitiesQueryRepository
{
    public async Task<IReadOnlyList<Activity>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default)
    {
        var daos = await dbContext.Activities
            .AsNoTracking()
            .Where(x => x.CardId == cardId)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Activity>>(daos);
    }
}
