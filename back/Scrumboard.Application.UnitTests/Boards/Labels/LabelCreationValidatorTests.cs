using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Application.Boards.Labels;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Labels;

public sealed class LabelCreationValidatorTests : UnitTestsBase
{
    private readonly CustomizedFixture _fixture = new();

    private readonly IBoardsRepository _boardsRepository = Mock.Of<IBoardsRepository>();
    
    private readonly IValidator<LabelCreation> _sut;

    public LabelCreationValidatorTests()
    {
        _sut = new LabelCreationValidator(_boardsRepository);
    }
    
    [Fact]
    public async Task Should_have_error_when_Name_exceed_255_characters()
    {
        // Arrange
        var labelCreation = _fixture.Build<LabelCreation>()
            .With(x => x.Name, new string('A', 256))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(labelCreation);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public async Task Should_have_error_when_BoardId_not_found()
    {
        // Arrange
        var labelCreation = _fixture.Build<LabelCreation>()
            .With(x => x.Name, new string('A', 255))
            .Create();
        
        Given_a_not_found_Board(labelCreation.BoardId);
        
        // Act
        var result = await _sut.TestValidateAsync(labelCreation);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BoardId);
    }
    
    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var labelCreation = _fixture.Build<LabelCreation>()
            .With(x => x.Name, new string('A', 50))
            .Create();
        
        Given_a_found_Board(labelCreation.BoardId);
        
        // Act
        var result = await _sut.TestValidateAsync(labelCreation);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    private void Given_a_found_Board(BoardId boardId)
    {
        var board = _fixture.Build<Board>()
            .With(x => x.Id, boardId)
            .Create();

        Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);
    }
    
    private void Given_a_not_found_Board(BoardId boardId) 
        => Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Board);
}
