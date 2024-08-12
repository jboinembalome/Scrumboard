using AutoFixture;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards.Activities;

public sealed class ActivityCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Activity>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.CardId)
            .Without(x => x.CreatedBy));
    }
}
