using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivitiesQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IActivitiesQueryRepository
{
    public async Task<IReadOnlyList<Activity>> GetByCardIdAsync(
        CardId cardId, 
        CancellationToken cancellationToken = default)
    {
        var activities = await dbContext.Activities
            .AsNoTracking()
            .Where(x => x.CardId == cardId)
            .ToListAsync(cancellationToken);

        return activities.Count > 0 
            ? mapper.Map<IReadOnlyList<Activity>>(activities)  
            : [];
    }
}
