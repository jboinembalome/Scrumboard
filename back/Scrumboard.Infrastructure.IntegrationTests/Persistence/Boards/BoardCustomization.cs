using AutoFixture;
using Scrumboard.Domain.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class BoardCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Board>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
