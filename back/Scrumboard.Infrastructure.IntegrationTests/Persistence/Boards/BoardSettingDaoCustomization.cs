using AutoFixture;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class BoardSettingDaoCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<BoardSettingDao>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.BoardId));
    }
}
