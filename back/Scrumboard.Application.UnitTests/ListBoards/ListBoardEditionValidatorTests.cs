using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Application.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.ListBoards;

public sealed class ListBoardEditionValidatorTests
{
    private readonly CustomizedFixture _fixture = new();

    private readonly IListBoardsRepository _listBoardsRepository = Mock.Of<IListBoardsRepository>();
    private readonly IBoardsRepository _boardsRepository = Mock.Of<IBoardsRepository>();

    private readonly IValidator<ListBoardEdition> _sut;

    public ListBoardEditionValidatorTests()
    {
        _sut = new ListBoardEditionValidator(
            _listBoardsRepository, 
            _boardsRepository);
    }

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>()
            .With(x => x.Name, string.Empty)
            .Create();

        // Act
        var result = await _sut.TestValidateAsync(listBoardEdition);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_exceeds_255_characters()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>()
            .With(x => x.Name, new string('A', 256))
            .Create();

        // Act
        var result = await _sut.TestValidateAsync(listBoardEdition);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Position_is_not_greater_than_0()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>()
            .With(x => x.Position, 0)
            .Create();

        // Act
        var result = await _sut.TestValidateAsync(listBoardEdition);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Position);
    }

    [Fact]
    public async Task Should_have_error_when_ListBoardId_not_found()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>()
            .With(x => x.Id, new ListBoardId(1))
            .Create();

        Given_a_not_found_ListBoard(listBoardEdition.Id);

        // Act
        var result = await _sut.TestValidateAsync(listBoardEdition);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public async Task Should_have_error_when_BoardId_not_found()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>()
            .With(x => x.BoardId, new BoardId(1))
            .Create();

        Given_a_not_found_Board(listBoardEdition.BoardId);

        // Act
        var result = await _sut.TestValidateAsync(listBoardEdition);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BoardId);
    }

    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>()
            .With(x => x.Name, new string('A', 255))
            .With(x => x.Position, 1)
            .Create();

        Given_a_found_ListBoard(listBoardEdition.Id);
        Given_a_found_Board(listBoardEdition.BoardId);

        // Act
        var result = await _sut.TestValidateAsync(listBoardEdition);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private void Given_a_found_ListBoard(ListBoardId listBoardId)
    {
        var listBoard = _fixture.Build<ListBoard>()
            .With(x => x.Id, listBoardId)
            .Create();

        Mock.Get(_listBoardsRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listBoard);
    }

    private void Given_a_not_found_ListBoard(ListBoardId listBoardId) 
        => Mock.Get(_listBoardsRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ListBoard);

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
