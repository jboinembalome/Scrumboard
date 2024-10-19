using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Application.Cards.Comments;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.SharedKernel.Exceptions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Comments;

public sealed class CommentsServiceTests
{
    private readonly IFixture _fixture;

    private readonly IMapper _mapper;
    private readonly ICommentsRepository _commentsRepository;
    private readonly ICommentsQueryRepository _commentsQueryRepository;
    private readonly IValidator<CommentCreation> _commentCreationValidator;
    private readonly IValidator<CommentEdition> _commentEditionValidator;

    private readonly CommentsService _sut;

    public CommentsServiceTests()
    {
        _fixture = new CustomizedFixture();

        _mapper = Mock.Of<IMapper>();
        _commentsRepository = Mock.Of<ICommentsRepository>();
        _commentsQueryRepository = Mock.Of<ICommentsQueryRepository>();
        _commentCreationValidator = Mock.Of<IValidator<CommentCreation>>();
        _commentEditionValidator = Mock.Of<IValidator<CommentEdition>>();

        _sut = new CommentsService(
            _mapper,
            _commentsRepository,
            _commentsQueryRepository,
            _commentCreationValidator,
            _commentEditionValidator);
    }

    [Fact]
    public async Task Get_by_Id_should_throw_an_exception_when_Comment_not_found()
    {
        // Arrange
        var commentId = _fixture.Create<CommentId>();
        
        Mock.Get(_commentsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Comment);

        // Act
        var act = async () => await _sut.GetByIdAsync(commentId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Get_by_Id_should_return_Comment_when_found()
    {
        // Arrange
        var commentId = _fixture.Create<CommentId>();
        var comment = _fixture.Create<Comment>();
        
        Mock.Get(_commentsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);

        // Act
        var result = await _sut.GetByIdAsync(commentId);

        // Assert
        result.Should()
            .Be(comment);
    }
    
    [Fact]
    public async Task Get_by_CardId_should_return_Comments()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var comments = _fixture.CreateMany<Comment>().ToList();

        Mock.Get(_commentsQueryRepository)
            .Setup(repo => repo.GetByCardIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comments);

        // Act
        var result = await _sut.GetByCardIdAsync(cardId);

        // Assert
        result.Should()
            .BeEquivalentTo(comments);
    }
    
    [Fact]
    public async Task Add_should_validate_CommentCreation()
    {
        // Arrange
        var commentCreation = _fixture.Create<CommentCreation>();
        var comment = _fixture.Create<Comment>();
        
        Given_mapping(commentCreation, comment);
        
        // Act
        await _sut.AddAsync(commentCreation);

        // Assert
        _commentCreationValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }
    
    [Fact]
    public async Task Add_should_not_proceed_when_validation_failed()
    {
        // Arrange
        var commentCreation = _fixture.Build<CommentCreation>()
            .With(x => x.Message, string.Empty)
            .Create();
        
        _commentCreationValidator.SetupValidationFailed(
            propertyName: nameof(CommentCreation.Message), 
            errorMessage: "Message is required."
        );
        
        // Act
        var act = async () => await _sut.AddAsync(commentCreation);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
        
        Mock.Get(_commentsRepository)
            .Verify(x => x.AddAsync(
                    It.IsAny<Comment>(), 
                    It.IsAny<CancellationToken>()), 
                Times.Never);
    }

    [Fact]
    public async Task Add_should_map_CommentCreation_to_Comment()
    {
        // Arrange
        var commentCreation = _fixture.Create<CommentCreation>();
        var comment = _fixture.Create<Comment>();
        
        Given_mapping(commentCreation, comment);

        // Act
        var result = await _sut.AddAsync(commentCreation);

        // Assert
        result.Should()
            .Be(comment);
        
        Mock.Get(_mapper)
            .Verify(x => x.Map<Comment>(commentCreation), Times.Once);
    }
    
    [Fact]
    public async Task Add_should_call_the_repository()
    {
        // Arrange
        var commentCreation = _fixture.Create<CommentCreation>();
        var comment = _fixture.Create<Comment>();
        
        Given_mapping(commentCreation, comment);

        // Act
        await _sut.AddAsync(commentCreation);

        // Assert
        Mock.Get(_commentsRepository)
            .Verify(x => x.AddAsync(
                    comment, 
                    It.IsAny<CancellationToken>()), 
                Times.Once);
    }
    
    [Fact]
    public async Task Update_should_validate_CommentEdition()
    {
        // Arrange
        var commentEdition = _fixture.Create<CommentEdition>();
        
        Given_a_found_Comment(commentEdition.Id);
        
        // Act
        await _sut.UpdateAsync(commentEdition);

        // Assert
        _commentEditionValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }
    
    [Fact]
    public async Task Update_should_not_proceed_when_validation_failed()
    {
        // Arrange
        var commentEdition = _fixture.Build<CommentEdition>()
            .With(x => x.Message, string.Empty)
            .Create();
        
        _commentEditionValidator.SetupValidationFailed(
            propertyName: nameof(CommentEdition.Message), 
            errorMessage: "Message is required."
        );
        
        // Act
        var act = async () => await _sut.UpdateAsync(commentEdition);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
        
        Mock.Get(_commentsRepository)
            .Verify(x => x.Update(
                    It.IsAny<Comment>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Update_should_not_proceed_when_Comment_not_found()
    {
        // Arrange
        var commentEdition = _fixture.Build<CommentEdition>().Create();
        
        Given_a_not_found_Comment(commentEdition.Id);

        // Act
        var act = async () => await _sut.UpdateAsync(commentEdition);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
        
        Mock.Get(_commentsRepository)
            .Verify(x => x.Update(
                    It.IsAny<Comment>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Update_should_call_the_repository()
    {
        // Arrange
        var commentEdition = _fixture.Build<CommentEdition>().Create();
        var comment = Given_a_found_Comment(commentEdition.Id);

        // Act
        await _sut.UpdateAsync(commentEdition);

        // Assert
        Mock.Get(_commentsRepository)
            .Verify(x => x.Update(comment), Times.Once);
    }
    
    [Fact]
    public async Task Update_should_update_the_comment()
    {
        // Arrange
        var commentEdition = _fixture.Build<CommentEdition>().Create();
        var comment = Given_a_found_Comment(commentEdition.Id);

        // Act
        await _sut.UpdateAsync(commentEdition);

        // Assert
        comment.Message.Should()
            .Be(commentEdition.Message);
    }
    
    [Fact]
    public async Task Delete_should_not_proceed_when_Comment_not_found()
    {
        // Arrange
        var commentId = _fixture.Create<CommentId>();
        
        Given_a_not_found_Comment(commentId);

        // Act
        var act = async () => await _sut.DeleteAsync(commentId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();

        Mock.Get(_commentsRepository)
            .Verify(x => x.DeleteAsync(
                    It.IsAny<CommentId>(), 
                    It.IsAny<CancellationToken>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Delete_should_call_the_repository()
    {
        // Arrange
        var commentId = _fixture.Create<CommentId>();
        
        Given_a_found_Comment(commentId);

        // Act
        await _sut.DeleteAsync(commentId);

        // Assert
        Mock.Get(_commentsRepository)
            .Verify(x => x.DeleteAsync(commentId, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    private void Given_mapping(CommentCreation commentCreation, Comment comment) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map<Comment>(commentCreation))
            .Returns(comment);

    private Comment Given_a_found_Comment(CommentId commentId)
    {
        var comment = _fixture.Build<Comment>()
            .With(x => x.Id, commentId)
            .Create();

        Mock.Get(_commentsRepository)
            .Setup(x => x.TryGetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        
        return comment;
    }
    
    private void Given_a_not_found_Comment(CommentId commentId) 
        => Mock.Get(_commentsRepository)
            .Setup(x => x.TryGetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Comment);
}
