using AutoFixture;
using Scrumboard.Infrastructure.Persistence.Cards.Activities;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards.Activities;

public sealed class ActivityDaoAppliedCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ActivityDao>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.CardId)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
