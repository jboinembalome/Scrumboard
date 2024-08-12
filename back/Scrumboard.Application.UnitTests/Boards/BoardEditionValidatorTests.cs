using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Boards;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards;

public sealed class BoardEditionValidatorTests
{
    private readonly CustomizedFixture _fixture = new();

    private readonly IBoardsRepository _boardsRepository = Mock.Of<IBoardsRepository>();
    
    private readonly IValidator<BoardEdition> _sut;

    public BoardEditionValidatorTests()
    {
        _sut = new BoardEditionValidator(_boardsRepository);
    }

    [Fact]
    public async Task Should_have_error_when_Name_exceed_50_characters()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>()
            .With(x => x.Name, new string('A', 51))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(boardEdition);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public async Task Should_have_error_when_Id_not_found()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>()
            .With(x => x.Name, new string('A', 51))
            .Create();
        
        Given_a_not_found_board(boardEdition.Id);
        
        // Act
        var result = await _sut.TestValidateAsync(boardEdition);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>()
            .With(x => x.Name, new string('A', 50))
            .Create();
        
        Given_a_found_board(boardEdition.Id);
        
        // Act
        var result = await _sut.TestValidateAsync(boardEdition);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    private void Given_a_found_board(BoardId boardId)
    {
        var board = _fixture.Build<Board>()
            .With(x => x.Id, boardId)
            .Create();

        Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, new CancellationToken()))
            .ReturnsAsync(board);
    }
    
    private void Given_a_not_found_board(BoardId boardId) 
        => Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, new CancellationToken()))
            .ReturnsAsync((Board?)null);
}
