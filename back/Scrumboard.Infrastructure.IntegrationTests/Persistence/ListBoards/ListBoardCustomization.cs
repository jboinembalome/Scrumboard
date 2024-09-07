using AutoFixture;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.ListBoards;

public sealed class ListBoardCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ListBoard>(transform => transform
            .FromFactory(() => new ListBoard(
                name: fixture.Create<string>(),          
                position: fixture.Create<int>(),        
                boardId: fixture.Create<BoardId>()))
            .Without(x => x.Id)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
