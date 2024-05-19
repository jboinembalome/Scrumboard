using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Scrumboard.Application.Boards.Specifications;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Specifications;

public class BoardsByUserIdSpecTests
{
    [Fact]
    public void BoardsByUserIdSpecTest_ExistingUserId_ReturnsBoard()
    {
        // Arrange
        var adherent1Model = new Adherent
        {
            Id = 1,
            IdentityId = "2cd08f87-33a6-4cbc-a0de-71d428986b85"
        };

        var adherent2Model = new Adherent
        {
            Id = 2,
            IdentityId = "3cd08f87-33a6-4cbc-a0de-71d428986b85"
        };

        var board1 = new Board { Adherent = adherent1Model };
        var board2 = new Board { Adherent = adherent1Model };
        var board3 = new Board { Adherent = adherent2Model };

        var boards = new List<Board>() { board1, board2, board3 };

        var specification = new BoardsByUserIdSpec(adherent1Model.IdentityId);

        // Act
        var filteredBoard = specification.Evaluate(boards).ToList();

        // Assert
        filteredBoard.Count.Should().Be(2);
        filteredBoard.Should().Contain(board1);
        filteredBoard.Should().Contain(board2);
        filteredBoard.Should().NotContain(board3);
    }
}