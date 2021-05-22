using FluentAssertions;
using Scrumboard.Application.Features.Boards.Specifications;
using Scrumboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Scrumboard.Application.UnitTests.Features.Boards.Specifications
{
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
            firstBoard.Id.Should().Be(boardId1);

            firstBoard.Labels.Count().Should().Be(1);
            firstBoard.Labels.FirstOrDefault().Should().NotBeNull();

            firstBoard.ListBoards.Count.Should().Be(1);
            firstBoard.ListBoards.FirstOrDefault().Should().NotBeNull();

            firstBoard.ListBoards.First().Cards.Count.Should().Be(1);
            firstBoard.ListBoards.First().Cards.FirstOrDefault().Should().NotBeNull();

            firstBoard.ListBoards.First().Cards.First().Labels.Count.Should().Be(1);
            firstBoard.ListBoards.First().Cards.First().Labels.FirstOrDefault().Should().NotBeNull();

            firstBoard.ListBoards.First().Cards.First().Attachments.Count.Should().Be(1);
            firstBoard.ListBoards.First().Cards.First().Attachments.FirstOrDefault().Should().NotBeNull();

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
                Labels = new Collection<Label>
                {
                    new Label { Name = "Label1"}
                },
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
                                },
                                Attachments = new Collection<Attachment>
                                {
                                    new Attachment { Name = "Attachment1"}
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
}
