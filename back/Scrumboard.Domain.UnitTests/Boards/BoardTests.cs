using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Events;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Domain.UnitTests.Boards;

public sealed class BoardTests
{
    private readonly IFixture _fixture = new CustomizedFixture();
    
    [Fact]
    public void MarkAsCreated_should_throw_an_exception_when_Id_is_null()
    {
        // Arrange
        var board = Given_a_Board_on_creation();

        // Act
        var act = () => board.MarkAsCreated();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Board ID must be set before marking as created.");
    }

    [Fact]
    public void MarkAsCreated_should_add_BoardCreatedDomainEvent()
    {
        // Arrange
        var board = Given_a_Board_on_creation();
        board.SetProperty(x => x.Id, _fixture.Create<BoardId>());
        
        // Act
        board.MarkAsCreated();

        // Assert
        var expectedBoardCreatedDomainEvent = new BoardCreatedDomainEvent(
            board.Id, 
            board.OwnerId);
        
        board.DomainEvents.Should()
            .ContainSingle()
            .Which
                .Should()
                .BeEquivalentTo(expectedBoardCreatedDomainEvent, opt => opt
                    .Excluding(x => x.DateOccurred));
    }
    
    [Fact]
    public void Update_should_correctly_apply_changes()
    {
        // Arrange
        var board = Given_a_Board_on_edition();
        
        var newName = _fixture.Create<string>();
        var newIsPinned = _fixture.Create<bool>();
        var newColour = _fixture.Create<Colour>();
        
        // Act
        board.Update(
            name: newName,
            isPinned: newIsPinned,
            boardSettingColour: newColour);

        // Assert
        var expectedBoard = new Board();
        expectedBoard.SetProperty(x => x.Id, board.Id);
        expectedBoard.SetProperty(x => x.OwnerId, board.OwnerId);
        expectedBoard.SetProperty(x => x.Name, newName);
        expectedBoard.SetProperty(x => x.IsPinned, newIsPinned);
        expectedBoard.BoardSetting.SetProperty(x => x.Colour, newColour);
        
        board.Should()
            .BeEquivalentTo(expectedBoard);
    }
    
    private Board Given_a_Board_on_creation() 
        => new(
            name: _fixture.Create<string>(),
            isPinned: _fixture.Create<bool>(),
            boardSetting: new BoardSetting(
                colour: _fixture.Create<Colour>()),
            ownerId: _fixture.Create<OwnerId>());
    
    private Board Given_a_Board_on_edition()
    {
        var board = Given_a_Board_on_creation();
        board.SetProperty(x => x.Id, _fixture.Create<BoardId>());
        
        return board;
    }
}
