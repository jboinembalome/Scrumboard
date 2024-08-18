using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence.Interceptors;

internal sealed class CreatedEntityInterceptor(
    ICurrentUserService currentUserService,
    ICurrentDateService currentDateService) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }
        
        foreach (var entry in context.ChangeTracker.Entries<ICreatedAtEntity>())
        {
            // TODO: Hack to not depend on httpContextAccessor when using ScrumboardDbContextSeed
            // (Will be removed later)
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (entry.State is not EntityState.Added || entry.Entity.CreatedBy.Value is not null)
            {
                continue;
            }

            entry.Entity.CreatedBy = (UserId)currentUserService.UserId;
            entry.Entity.CreatedDate = currentDateService.Now;
        }
    }
}
