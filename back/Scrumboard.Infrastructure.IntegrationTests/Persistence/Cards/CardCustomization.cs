using AutoFixture;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards;

public sealed class CardCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Card>(transform => transform
            .FromFactory(() => new Card(
                name: fixture.Create<string>(),
                description: fixture.Create<string>(),
                dueDate: fixture.Create<DateTimeOffset>(),
                position: fixture.Create<int>(),
                listBoardId: fixture.Create<ListBoardId>(),
                assigneeIds: [],
                labelIds: []))
            .Without(x => x.Id)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
