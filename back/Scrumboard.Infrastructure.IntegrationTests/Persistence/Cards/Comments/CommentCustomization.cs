using AutoFixture;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Shared.TestHelpers.Fixtures;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards.Comments;

public sealed class CommentCustomization : IAutoAppliedCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Comment>(transform => transform
            .FromFactory(() => new Comment(
                message: fixture.Create<string>(),          
                cardId: fixture.Create<CardId>()))
            .Without(x => x.Id)
            .Without(x => x.CreatedBy));
    }
}
