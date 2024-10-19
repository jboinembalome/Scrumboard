using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Application.Cards.Comments;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Comments;

public sealed class CommentEditionValidatorTests
{
    private readonly CustomizedFixture _fixture = new();
    
    private readonly ICommentsRepository _commentsRepository = Mock.Of<ICommentsRepository>();
    private readonly ICardsRepository _cardsRepository = Mock.Of<ICardsRepository>();
    
    private readonly IValidator<CommentEdition> _sut;

    public CommentEditionValidatorTests()
    {
        _sut = new CommentEditionValidator(
            _commentsRepository,
            _cardsRepository);
    }
    
    [Fact]
    public async Task Should_have_error_when_Message_exceed_500_characters()
    {
        // Arrange
        var commentCreation = _fixture.Build<CommentEdition>()
            .With(x => x.Message, new string('A', 501))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(commentCreation);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Message);
    }
    
    [Fact]
    public async Task Should_have_error_when_Id_not_found()
    {
        // Arrange
        var commentCreation = _fixture.Build<CommentEdition>()
            .With(x => x.Id, new CommentId(1))
            .Create();

        Given_a_not_found_Comment(commentCreation.Id);

        // Act
        var result = await _sut.TestValidateAsync(commentCreation);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CardId);
    }
    
    [Fact]
    public async Task Should_have_error_when_CardId_not_found()
    {
        // Arrange
        var commentCreation = _fixture.Build<CommentEdition>()
            .With(x => x.CardId, new CardId(1))
            .Create();

        Given_a_not_found_Card(commentCreation.CardId);

        // Act
        var result = await _sut.TestValidateAsync(commentCreation);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CardId);
    }
    
    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var commentCreation = _fixture.Build<CommentEdition>()
            .With(x => x.Id, new CommentId(1))
            .With(x => x.Message, new string('A', 500))
            .With(x => x.CardId, new CardId(1))
            .Create();
        
        Given_a_found_Comment(commentCreation.Id);
        Given_a_found_Card(commentCreation.CardId);
        
        // Act
        var result = await _sut.TestValidateAsync(commentCreation);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    private void Given_a_found_Comment(CommentId commentId)
    {
        var comment = _fixture.Build<Comment>()
            .With(x => x.Id, commentId)
            .Create();

        Mock.Get(_commentsRepository)
            .Setup(x => x.TryGetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
    }

    private void Given_a_not_found_Comment(CommentId commentId) 
        => Mock.Get(_commentsRepository)
            .Setup(x => x.TryGetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Comment);
    
    private void Given_a_found_Card(CardId cardId)
    {
        var card = _fixture.Build<Card>()
            .With(x => x.Id, cardId)
            .Create();

        Mock.Get(_cardsRepository)
            .Setup(x => x.TryGetByIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);
    }

    private void Given_a_not_found_Card(CardId cardId) 
        => Mock.Get(_cardsRepository)
            .Setup(x => x.TryGetByIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Card);
}
