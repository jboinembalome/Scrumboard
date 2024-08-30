using AutoFixture;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards.Labels;

public sealed class LabelCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Label>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.BoardId)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
