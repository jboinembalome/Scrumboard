using AutoFixture;
using Scrumboard.Domain.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class BoardSettingCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<BoardSetting>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.BoardId));
    }
}
