using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Application.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.ListBoards;

public sealed class ListBoardCreationValidatorTests
{
    private readonly CustomizedFixture _fixture = new();
    
    private readonly IBoardsRepository _boardsRepository = Mock.Of<IBoardsRepository>();
    
    private readonly IValidator<ListBoardCreation> _sut;

    public ListBoardCreationValidatorTests()
    {
        _sut = new ListBoardCreationValidator(_boardsRepository);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        // Arrange
        var listBoardCreation = _fixture.Build<ListBoardCreation>()
            .With(x => x.Name, string.Empty)
            .Create();

        // Act
        var result = await _sut.TestValidateAsync(listBoardCreation);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_exceeds_255_characters()
    {
        // Arrange
        var listBoardCreation = _fixture.Build<ListBoardCreation>()
            .With(x => x.Name, new string('A', 256))
            .Create();

        // Act
        var result = await _sut.TestValidateAsync(listBoardCreation);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Position_is_not_greater_than_0()
    {
        // Arrange
        var listBoardCreation = _fixture.Build<ListBoardCreation>()
            .With(x => x.Position, 0)
            .Create();

        // Act
        var result = await _sut.TestValidateAsync(listBoardCreation);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Position);
    }

    [Fact]
    public async Task Should_have_error_when_BoardId_not_found()
    {
        // Arrange
        var listBoardCreation = _fixture.Build<ListBoardCreation>()
            .With(x => x.BoardId, new BoardId(1))
            .Create();

        Given_a_not_found_Board(listBoardCreation.BoardId);

        // Act
        var result = await _sut.TestValidateAsync(listBoardCreation);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BoardId);
    }

    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var listBoardCreation = _fixture.Build<ListBoardCreation>()
            .With(x => x.Name, new string('A', 255))
            .With(x => x.Position, 1)
            .With(x => x.BoardId, new BoardId(1))
            .Create();

        Given_a_found_Board(listBoardCreation.BoardId);

        // Act
        var result = await _sut.TestValidateAsync(listBoardCreation);

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
