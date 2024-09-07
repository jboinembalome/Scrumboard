using AutoFixture;
using Scrumboard.Domain.Cards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards;

public sealed class CardAssigneeCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CardAssignee>(transform => transform
            .Without(x => x.CardId)
            .With(x => x.AssigneeId, Guid.NewGuid().ToString()));
    }
}
