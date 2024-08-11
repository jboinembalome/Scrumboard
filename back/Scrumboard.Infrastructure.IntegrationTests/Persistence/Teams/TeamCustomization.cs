using AutoFixture;
using Scrumboard.Domain.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class TeamCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Team>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.CreatedBy));
    }
}
