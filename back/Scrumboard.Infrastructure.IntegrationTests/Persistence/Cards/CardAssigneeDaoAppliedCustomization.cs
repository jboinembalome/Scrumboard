using AutoFixture;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards;

public sealed class CardAssigneeDaoAppliedCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CardAssigneeDao>(transform => transform
            .Without(x => x.CardId)
            .With(x => x.AssigneeId, Guid.NewGuid().ToString()));
    }
}
