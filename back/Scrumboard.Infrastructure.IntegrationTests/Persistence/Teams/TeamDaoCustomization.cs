using AutoFixture;
using Scrumboard.Infrastructure.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class TeamDaoCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TeamDao>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
