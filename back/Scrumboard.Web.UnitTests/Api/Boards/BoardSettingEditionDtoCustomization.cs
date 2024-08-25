using AutoFixture;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.Web.Api.Boards;

namespace Scrumboard.Web.UnitTests.Api.Boards;

public sealed class BoardSettingEditionDtoCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<BoardSettingEditionDto>(transform => transform
            .With(x => x.Colour, fixture.Create<Colour>().Code));
    }
}
