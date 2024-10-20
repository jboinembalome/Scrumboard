namespace Scrumboard.Infrastructure.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(Func<Task> funcTask, CancellationToken cancellationToken = default);
}
