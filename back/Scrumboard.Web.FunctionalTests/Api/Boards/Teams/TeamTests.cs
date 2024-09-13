using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Teams;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Boards.Teams;

public sealed class TeamTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_teams_by_boardId_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_Board(SeededUser.Adherent.Id);
        await Given_a_Team(board.Id, [SeededUser.Adherent.Id]);

        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}/teams");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_teams_by_boardId_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_Board(SeededUser.NoRightUser.Id);
        
        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}/teams");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private Task<Board> Given_a_Board(OwnerId ownerId) 
        => _factory.AddEntityAsync(new Board(
            name: "Test Board",
            isPinned: false,
            boardSetting: new BoardSetting(
                colour: Colour.Blue),
            ownerId: ownerId));

    private Task<Team> Given_a_Team(BoardId boardId, IEnumerable<MemberId> memberIds) 
        => _factory.AddEntityAsync(new Team(
            name: "Test Team",
            boardId: boardId,
            memberIds: memberIds));
}
