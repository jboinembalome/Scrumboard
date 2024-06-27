using System.Collections.ObjectModel;
using FluentAssertions;
using Scrumboard.Application.Boards.Specifications;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.ListBoards;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Specifications;

public class BoardWithAllSpecTests
{
    private readonly int boardId1 = 1;

    [Fact]
    public void BoardWithAllSpecTest_ExistingUserId_ReturnsBoard()
    {
        // Arrange
        var boards = GetTestBoardsCollection();

        var specification = new BoardWithAllSpec(boardId1);

        // Act
        var firstBoard = specification.Evaluate(boards).FirstOrDefault();

        // Assert
        firstBoard!.Id.Should().Be(boardId1);

        firstBoard.ListBoards.Count.Should().Be(1);
        firstBoard.ListBoards.FirstOrDefault().Should().NotBeNull();

        firstBoard.ListBoards.First().Cards.Count.Should().Be(1);
        firstBoard.ListBoards.First().Cards.FirstOrDefault().Should().NotBeNull();

        firstBoard.ListBoards.First().Cards.First().Labels.Count.Should().Be(1);
        firstBoard.ListBoards.First().Cards.First().Labels.FirstOrDefault().Should().NotBeNull();

        firstBoard.ListBoards.First().Cards.First().Checklists.Count.Should().Be(1);
        firstBoard.ListBoards.First().Cards.First().Checklists.FirstOrDefault().Should().NotBeNull();

        firstBoard.ListBoards.First().Cards.First().Checklists.First().ChecklistItems.Count.Should().Be(1);
        firstBoard.ListBoards.First().Cards.First().Checklists.First().ChecklistItems.FirstOrDefault().Should().NotBeNull();

        firstBoard.ListBoards.First().Cards.First().Comments.Count.Should().Be(1);
        firstBoard.ListBoards.First().Cards.First().Comments.FirstOrDefault().Should().NotBeNull();
    }

    public List<Board> GetTestBoardsCollection()
    {
        var boardId2 = 2;

        var board1 = new Board
        {
            Id = boardId1,
            ListBoards = new Collection<ListBoard>
            {
                new ListBoard
                {
                    Name = "Lisboard1",
                    Cards = new Collection<Card>
                    {
                        new Card
                        {
                            Name = "Card1",
                            Checklists = new Collection<Checklist>
                            {
                                new Checklist
                                {
                                    Name = "Checklist1",
                                    ChecklistItems = new Collection<ChecklistItem>
                                    {
                                        new ChecklistItem { Name = "ChecklistItem1"}
                                    }
                                }
                            },
                            Labels = new Collection<Label>
                            {
                                new Label { Name = "Label1"}
                            },
                            Comments = new Collection<Comment>
                            {
                                new Comment { Message = "Comment1"}
                            }
                        }
                    }
                }
            }
        };
        var board2 = new Board { Id = boardId2 };

        var boards = new List<Board>() { board1, board2 };

        return boards;
    }
}
