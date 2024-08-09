using AutoFixture;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards;

public sealed class CardDaoAppliedCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CardDao>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.ListBoardId)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
