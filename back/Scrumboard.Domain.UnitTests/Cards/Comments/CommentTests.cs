using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Domain.UnitTests.Cards.Comments;

public class CommentTests
{
    private readonly IFixture _fixture = new CustomizedFixture();
    
    [Fact]
    public void Update_should_only_set_Message()
    {
        // Arrange
        var comment = Given_a_Comment();
        var newMessage = _fixture.Create<string>();

        // Act
        comment.Update(newMessage);

        // Assert
        var expectedComment = new Comment(
            newMessage, 
            comment.CardId);
        
        comment.Should()
            .BeEquivalentTo(expectedComment);
    }
    
    private Comment Given_a_Comment() 
        => new(
            message: _fixture.Create<string>(),
            cardId: _fixture.Create<CardId>());
}
