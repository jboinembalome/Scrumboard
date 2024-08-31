using AutoFixture;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.Web.Api.Boards.Labels;

namespace Scrumboard.Web.UnitTests.Api.Boards.Labels;

public sealed class LabelCreationDtoCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LabelCreationDto>(transform => transform
            .With(x => x.Colour, fixture.Create<Colour>().Code));
    }
}
