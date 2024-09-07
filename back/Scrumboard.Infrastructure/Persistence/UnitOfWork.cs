using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Scrumboard.Infrastructure.Abstractions.Persistence;
using IsolationLevel = System.Data.IsolationLevel;

namespace Scrumboard.Infrastructure.Persistence;

internal sealed class UnitOfWork(
    ScrumboardDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(Func<Task> funcTask, CancellationToken cancellationToken = default)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await BeginTransactionAsync(cancellationToken);

            await funcTask();

            // Save the entities (domain events are published before the save)
            await dbContext.SaveChangesAsync(cancellationToken);

            // Commit all changes
            await transaction.CommitAsync(cancellationToken);

            // Call the outbox dispatcher here for integration events
        });
    }
    
    private async Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default)
        => await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
}
