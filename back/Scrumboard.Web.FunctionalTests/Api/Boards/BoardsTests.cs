using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Web.Api.Boards;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Boards;

public sealed class BoardsTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_boards_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        // Act
        var response = await client.GetAsync("/api/boards");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_boards_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);

        // Act
        var response = await client.GetAsync("/api/boards");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_board_by_id_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        var board = await Given_a_board(SeededUser.Adherent.Id);
        
        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_board_by_id_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        var board = await Given_a_board(SeededUser.NoRightUser.Id);

        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Post_board_should_return_Created()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        var payload = BuildPostBoardPayload();
        
        // Act
        var response = await client.PostAsJsonAsync("/api/boards", payload);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Post_board_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        var payload = BuildPostBoardPayload();

        // Act
        var response = await client.PostAsJsonAsync("/api/boards", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Put_board_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        var board = await Given_a_board(SeededUser.Adherent.Id);
        var payload = BuildPutBoardPayload(board.Id);
        
        // Act
        var response = await client.PutAsJsonAsync($"/api/boards/{board.Id}", payload);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Put_board_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        var board = await Given_a_board(SeededUser.NoRightUser.Id);
        var payload = BuildPutBoardPayload(board.Id);

        // Act
        var response = await client.PutAsJsonAsync($"/api/boards/{board.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<Board> Given_a_board(OwnerId ownerId)
        => await _factory.AddEntityAsync(new Board(
            name: "New Board",
            isPinned: false,
            boardSetting: new BoardSetting(
                colour: Colour.Blue),
            ownerId: ownerId));
    
    private static BoardCreationDto BuildPostBoardPayload()
        => new()
        {
            Name = "New Board",
            IsPinned = false,
            BoardSetting = new BoardSettingCreationDto
            {
                Colour = Colour.Blue
            }
        };
    
    private static BoardEditionDto BuildPutBoardPayload(BoardId boardId)
        => new()
        {
            Id = boardId.Value,
            Name = "New Board",
            IsPinned = false,
            BoardSetting = new BoardSettingEditionDto
            {
                Colour = Colour.Blue
            }
        };
}
