using AutoFixture;
using Scrumboard.Domain.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class TeamMemberCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TeamMember>(transform => transform
            .Without(x => x.TeamId)
            .With(x => x.MemberId, Guid.NewGuid().ToString()));
    }
}
