using FluentAssertions;
using Scrumboard.SharedKernel.DomainEvents;
using Xunit;

namespace Scrumboard.Infrastructure.UnitTests;

[CollectionDefinition("Global Test Setup")]
public class DatabaseCollection : ICollectionFixture<GlobalTestSetupFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class GlobalTestSetupFixture
{
    static GlobalTestSetupFixture()
    {
        // Exclude DomainEvents property for all tests:
        // E.g: entity.Should().BeEquivalentTo(expectedEntity);
        // expectedEntity will create a new DateOccurred for each domain event
        AssertionOptions.AssertEquivalencyUsing(options => options
            .Excluding(x => x.Path == (nameof(HasDomainEventsBase.DomainEvents))));
    }
}
