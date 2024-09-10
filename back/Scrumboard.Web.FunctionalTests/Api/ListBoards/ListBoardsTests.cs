using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Web.Api.ListBoards;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.ListBoards;

public sealed class ListBoardsTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_listBoard_by_id_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var listBoard = await Given_a_ListBoard(SeededUser.Adherent.Id);

        // Act
        var response = await client.GetAsync($"/api/list-boards/{listBoard.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_listBoard_by_id_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var listBoard = await Given_a_ListBoard(SeededUser.NoRightUser.Id);

        // Act
        var response = await client.GetAsync($"/api/list-boards/{listBoard.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Post_listBoard_should_return_Created()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var listBoard = await Given_a_ListBoard(SeededUser.Adherent.Id);
        
        var payload = BuildPostListBoardPayload(boardId: listBoard.BoardId.Value);

        // Act
        var response = await client.PostAsJsonAsync("/api/list-boards", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_listBoard_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var listBoard = await Given_a_ListBoard(SeededUser.NoRightUser.Id);
        
        var payload = BuildPostListBoardPayload(boardId: listBoard.BoardId.Value);

        // Act
        var response = await client.PostAsJsonAsync("/api/list-boards", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_listBoard_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var listBoard = await Given_a_ListBoard(SeededUser.Adherent.Id);
        
        var payload = BuildPutListBoardPayload(
            listBoardId: listBoard.Id.Value, 
            boardId: listBoard.BoardId.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/list-boards/{listBoard.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_listBoard_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var listBoard = await Given_a_ListBoard(SeededUser.NoRightUser.Id);
        
        var payload = BuildPutListBoardPayload(
            listBoardId: listBoard.Id.Value, 
            boardId: listBoard.BoardId.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/list-boards/{listBoard.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_listBoard_should_return_NoContent()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var listBoard = await Given_a_ListBoard(SeededUser.Adherent.Id);

        // Act
        var response = await client.DeleteAsync($"/api/list-boards/{listBoard.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_listBoard_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var listBoard = await Given_a_ListBoard(SeededUser.NoRightUser.Id);

        // Act
        var response = await client.DeleteAsync($"/api/list-boards/{listBoard.Id}");

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
    
    private async Task<ListBoard> Given_a_ListBoard(OwnerId ownerId)
    {
        var board = await Given_a_Board(ownerId);

        return await _factory.AddEntityAsync(new ListBoard(
            name: "Test ListBoard",
            position: 1,
            boardId: board.Id));
    }
    
    private static ListBoardCreationDto BuildPostListBoardPayload(
        int boardId) 
        => new()
        {
            Name = "New ListBoard",
            Position = 2,
            BoardId = boardId
        };
    
    private static ListBoardEditionDto BuildPutListBoardPayload(
        int listBoardId,
        int boardId) 
        => new()
        {
            Id = listBoardId,
            Name = "Updated ListBoard",
            Position = 2,
            BoardId = boardId
        };
}
