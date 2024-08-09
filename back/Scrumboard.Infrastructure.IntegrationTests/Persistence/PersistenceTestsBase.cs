using Scrumboard.Infrastructure.Persistence;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

[Collection("Database collection")]
public abstract class PersistenceTestsBase(
    DatabaseFixture databaseFixture) : IAsyncLifetime
{
    protected ScrumboardDbContext ArrangeDbContext { get; } = databaseFixture.CreateDbContext();
    protected ScrumboardDbContext ActDbContext { get; } = databaseFixture.CreateDbContext();
    protected ScrumboardDbContext AssertDbContext { get; } = databaseFixture.CreateDbContext();
    
    protected void SetCurrentUser(string userId) 
        => databaseFixture.CurrentUserServiceMock
            .Setup(m => m.UserId)
            .Returns(userId);
    
    protected void SetCurrentDate(DateTimeOffset dateTimeOffset) 
        => databaseFixture.CurrentDateServiceMock
            .Setup(m => m.Now)
            .Returns(dateTimeOffset); 
    
    public virtual async Task DisposeAsync()
    {
        await ArrangeDbContext.DisposeAsync();
        await ActDbContext.DisposeAsync();
        await AssertDbContext.DisposeAsync();
        
        await DatabaseFixture.ResetState();
    }

    public virtual Task InitializeAsync() 
        => Task.CompletedTask;
}
