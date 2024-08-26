using AutoFixture;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class BoardSettingCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<BoardSetting>(transform => transform
            .FromFactory(() => new BoardSetting(
                colour: fixture.Create<Colour>()))
            .Without(x => x.Id));
    }
}
