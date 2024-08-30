using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Teams;
using Scrumboard.Web.Api.Teams;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Teams;

public sealed class TeamTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_team_by_id_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_board(SeededUser.Adherent.Id);
        var team = await Given_a_team(SeededUser.Adherent.Id, board.Id);

        // Act
        var response = await client.GetAsync($"/api/teams/{team.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_team_by_id_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_board(SeededUser.NoRightUser.Id);
        var team = await Given_a_team(SeededUser.NoRightUser.Id, board.Id);

        // Act
        var response = await client.GetAsync($"/api/teams/{team.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Post_team_should_return_Created()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_board(SeededUser.Adherent.Id);
        var payload = BuildPostTeamPayload([SeededUser.Adherent.Id], board.Id.Value);

        // Act
        var response = await client.PostAsJsonAsync("/api/teams", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_team_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_board(SeededUser.NoRightUser.Id);
        var payload = BuildPostTeamPayload([SeededUser.NoRightUser.Id], board.Id.Value);

        // Act
        var response = await client.PostAsJsonAsync("/api/teams", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_team_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_board(SeededUser.Adherent.Id);
        var team = await Given_a_team(SeededUser.Adherent.Id, board.Id);
        var payload = BuildPutTeamPayload(team.Id.Value, [SeededUser.Adherent.Id], board.Id.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/teams/{team.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_team_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_board(SeededUser.NoRightUser.Id);
        var team = await Given_a_team(SeededUser.NoRightUser.Id, board.Id);
        
        var payload = BuildPutTeamPayload(team.Id.Value, [SeededUser.NoRightUser.Id], board.Id.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/teams/{team.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_team_should_return_NoContent()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_board(SeededUser.Adherent.Id);
        var team = await Given_a_team(SeededUser.Adherent.Id, board.Id);

        // Act
        var response = await client.DeleteAsync($"/api/teams/{team.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_team_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_board(SeededUser.NoRightUser.Id);
        var team = await Given_a_team(SeededUser.NoRightUser.Id, board.Id);

        // Act
        var response = await client.DeleteAsync($"/api/teams/{team.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<Team> Given_a_team(MemberId memberId, BoardId boardId)
    {
        var team = new Team(
            name: "New Team",
            boardId: boardId);
        
        team.AddMembers([memberId]);
        
        return await _factory.AddEntityAsync(team);
    }

    private Task<Board> Given_a_board(OwnerId ownerId)
        => _factory.AddEntityAsync(new Board(
            name: "New Board",
            isPinned: false,
            boardSetting: new BoardSetting(
                colour: Colour.Blue),
            ownerId: ownerId));

    private static TeamCreationDto BuildPostTeamPayload(
        IReadOnlyCollection<string> memberIds, 
        int boardId) 
        => new()
        {
            Name = "New Team",
            MemberIds = memberIds,
            BoardId = boardId
        };

    private static TeamEditionDto BuildPutTeamPayload(
        int teamId, 
        IReadOnlyCollection<string> memberIds, 
        int boardId) 
        => new()
        {
            Id = teamId,
            Name = "Updated Team",
            MemberIds = memberIds,
            BoardId = boardId
        };
}
