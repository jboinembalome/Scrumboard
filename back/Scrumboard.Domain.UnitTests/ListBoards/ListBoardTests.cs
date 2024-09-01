using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Domain.UnitTests.ListBoards;

public sealed class ListBoardTests
{
    private readonly IFixture _fixture = new CustomizedFixture();
    
    [Fact]
    public void Update_should_correctly_apply_changes()
    {
        // Arrange
        var listBoard = Given_a_ListBoard_on_edition();
        
        var newName = _fixture.Create<string>();
        var newPosition = _fixture.Create<int>();
        
        // Act
        listBoard.Update(
            name: newName,
            position: newPosition);

        // Assert
        var expectedListBoard = new ListBoard();
        expectedListBoard.SetProperty(x => x.Id, listBoard.Id);
        expectedListBoard.SetProperty(x => x.Name, newName);
        expectedListBoard.SetProperty(x => x.Position, newPosition);
        expectedListBoard.SetProperty(x => x.BoardId, listBoard.BoardId);
        expectedListBoard.SetProperty(x => x.Cards, listBoard.Cards);
        
        listBoard.Should()
            .BeEquivalentTo(expectedListBoard);
    }
    
    private ListBoard Given_a_ListBoard_on_edition()
    {
        var listBoard = new ListBoard(
            name: _fixture.Create<string>(),
            position: _fixture.Create<int>(),
            boardId: _fixture.Create<BoardId>());
        
        listBoard.SetProperty(x => x.Id, _fixture.Create<ListBoardId>());
        
        return listBoard;
    }
}
