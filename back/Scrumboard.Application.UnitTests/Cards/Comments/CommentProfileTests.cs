using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Application.Cards.Comments;
using Scrumboard.Domain.Cards.Comments;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Comments;

public sealed class CommentProfileTests
{
    private readonly IFixture _fixture;
    
    private readonly IMapper _mapper;
    
    public CommentProfileTests()
    {
        _fixture = new Fixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CommentProfile>();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void Should_map_CommentCreation_to_Comment()
    {
        // Arrange
        var commentCreation = _fixture.Create<CommentCreation>();
        
        // Act
        var comment = _mapper.Map<Comment>(commentCreation);
        
        // Assert
        var expectedComment = new Comment(
            message: commentCreation.Message,
            cardId: commentCreation.CardId);

        comment.Should()
            .BeEquivalentTo(expectedComment);
    }
}
