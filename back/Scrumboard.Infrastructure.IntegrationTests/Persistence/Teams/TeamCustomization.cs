using AutoFixture;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class TeamCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Team>(transform => transform
            .FromFactory(() => new Team(
                name: fixture.Create<string>(),          
                boardId: fixture.Create<BoardId>(),
                memberIds: fixture.CreateMany<MemberId>().ToArray()))
            .Without(x => x.Id)
            .Without(x => x.CreatedBy));
    }
}
