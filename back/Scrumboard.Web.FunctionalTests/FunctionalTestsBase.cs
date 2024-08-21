using Xunit;

namespace Scrumboard.Web.FunctionalTests;

[CollectionDefinition("Functional test collection")]
public sealed class FunctionalTestCollection : ICollectionFixture<CustomWebApplicationFactory>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

[Collection("Functional test collection")]
public abstract class FunctionalTestsBase(
    CustomWebApplicationFactory factory) : IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory _factory = factory;
    
    public virtual Task DisposeAsync() 
        => _factory.ResetDatabaseAsync();

    public virtual Task InitializeAsync() 
        => Task.CompletedTask;
}
