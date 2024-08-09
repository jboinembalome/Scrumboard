using AutoFixture;
using Scrumboard.Infrastructure.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class TeamMemberDaoAppliedCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TeamMemberDao>(transform => transform
            .Without(x => x.TeamId)
            .With(x => x.MemberId, Guid.NewGuid().ToString()));
    }
}
