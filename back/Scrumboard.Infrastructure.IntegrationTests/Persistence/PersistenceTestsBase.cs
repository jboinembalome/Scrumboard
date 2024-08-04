using Scrumboard.Infrastructure.Persistence;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

[Collection("Database collection")]
public abstract class PersistenceTestsBase(
    DatabaseFixture databaseFixture) : IAsyncLifetime
{
    protected ScrumboardDbContext DbContext => databaseFixture.DbContext;
    
    public virtual Task DisposeAsync() 
        => DatabaseFixture.ResetState();

    public virtual Task InitializeAsync() 
        => Task.CompletedTask;
}
