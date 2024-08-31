using AutoFixture;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards.Labels;

public sealed class LabelCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Label>(transform => transform
            .FromFactory(() => new Label(
                name: fixture.Create<string>(),
                colour: fixture.Create<Colour>(),
                boardId: fixture.Create<BoardId>()))
            .Without(x => x.Id)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
