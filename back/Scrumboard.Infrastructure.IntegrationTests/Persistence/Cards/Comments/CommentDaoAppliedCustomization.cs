using AutoFixture;
using Scrumboard.Infrastructure.Persistence.Cards.Comments;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards.Comments;

public sealed class CommentDaoAppliedCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CommentDao>(transform => transform
            .Without(x => x.Id)
            .Without(x => x.CardId)
            .Without(x => x.CreatedBy)
            .Without(x => x.LastModifiedBy));
    }
}
