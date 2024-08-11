using AutoFixture;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.ListBoards;

public sealed class ListBoardCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ListBoard>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.BoardId)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
