using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence.Interceptors;

internal sealed class ModifiedEntityInterceptor(
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
        
        foreach (var entry in context.ChangeTracker.Entries<IModifiedAtEntity>())
        {
            if (entry.State is not EntityState.Modified)
            {
                continue;
            }

            entry.Entity.LastModifiedBy = (UserId)currentUserService.UserId;
            entry.Entity.LastModifiedDate = currentDateService.Now;
            break;
        }
    }
}
