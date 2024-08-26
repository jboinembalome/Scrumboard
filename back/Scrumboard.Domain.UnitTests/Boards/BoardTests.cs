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
    
    private Board Given_a_Board_on_creation() 
        => new(
            name: _fixture.Create<string>(),
            isPinned: _fixture.Create<bool>(),
            boardSetting: new BoardSetting(
                colour: _fixture.Create<Colour>()),
            ownerId: _fixture.Create<OwnerId>());
}
