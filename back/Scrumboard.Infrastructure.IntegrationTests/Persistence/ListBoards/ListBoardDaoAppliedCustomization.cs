using AutoFixture;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.ListBoards;

public sealed class ListBoardDaoAppliedCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ListBoardDao>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.BoardId)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
