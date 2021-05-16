using FluentAssertions;
using Scrumboard.Application.Specifications;
using Scrumboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Scrumboard.Application.Tests.Features.Boards.Specifications
{
    public class BoardsByUserIdSpecTests
    {
        [Fact]
        public void BoardsByUserIdSpecTest_ExistingUserId_ReturnsBoard()
        {
            // Arrange
            var userId1 = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85");
            var userId2 = Guid.Parse("3cd08f87-33a6-4cbc-a0de-71d428986b85");

            var board1 = new Board { UserId = userId1 };
            var board2 = new Board { UserId = userId1 };
            var board3 = new Board { UserId = userId2 };

            var boards = new List<Board>() { board1, board2, board3 };

            var specification = new BoardsByUserIdSpec(userId1);

            // Act
            var filteredBoard = specification.Evaluate(boards).ToList();

            // Assert
            filteredBoard.Count().Should().Be(2);
            filteredBoard.Should().Contain(board1);
            filteredBoard.Should().Contain(board2);
            filteredBoard.Should().NotContain(board3);
        }
    }
}
