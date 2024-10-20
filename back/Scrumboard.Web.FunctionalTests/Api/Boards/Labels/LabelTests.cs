using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Common;
using Scrumboard.Web.Api.Boards.Labels;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Boards.Labels;

public sealed class LabelTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_labels_by_boardId_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_Board(SeededUser.Adherent.Id);
        await Given_a_Label(board.Id);

        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}/labels");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_labels_by_boardId_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_Board(SeededUser.NoRightUser.Id);
        
        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}/labels");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_label_by_id_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_Board(SeededUser.Adherent.Id);
        var label = await Given_a_Label(board.Id);

        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}/labels/{label.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_label_by_id_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_Board(SeededUser.NoRightUser.Id);
        var label = await Given_a_Label(board.Id);

        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}/labels/{label.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Post_label_should_return_Created()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_Board(SeededUser.Adherent.Id);
        
        var payload = BuildPostLabelPayload(boardId: board.Id.Value);

        // Act
        var response = await client.PostAsJsonAsync($"/api/boards/{board.Id}/labels", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Post_label_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_Board(SeededUser.NoRightUser.Id);
        
        var payload = BuildPostLabelPayload(boardId: board.Id.Value);

        // Act
        var response = await client.PostAsJsonAsync($"/api/boards/{board.Id}/labels", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Put_label_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_Board(SeededUser.Adherent.Id);
        var label = await Given_a_Label(board.Id);
        
        var payload = BuildPutLabelPayload(
            labelId: label.Id.Value, 
            boardId: board.Id.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/boards/{board.Id}/labels/{label.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Put_label_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_Board(SeededUser.NoRightUser.Id);
        var label = await Given_a_Label(board.Id);
        
        var payload = BuildPutLabelPayload(
            labelId: label.Id.Value, 
            boardId: board.Id.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/boards/{board.Id}/labels/{label.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Delete_label_should_return_NoContent()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var board = await Given_a_Board(SeededUser.Adherent.Id);
        var label = await Given_a_Label(board.Id);

        // Act
        var response = await client.DeleteAsync($"/api/boards/{board.Id}/labels/{label.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Delete_label_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_Board(SeededUser.NoRightUser.Id);
        var label = await Given_a_Label(board.Id);

        // Act
        var response = await client.DeleteAsync($"/api/boards/{board.Id}/labels/{label.Id}");

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

    private Task<Label> Given_a_Label(BoardId boardId) 
        => _factory.AddEntityAsync(new Label(
            name: "Test Label",
            colour: Colour.Red,
            boardId: boardId));

    private static LabelCreationDto BuildPostLabelPayload(int boardId) 
        => new()
        {
            Name = "New Label",
            Colour = Colour.Green,
            BoardId = boardId
        };

    private static LabelEditionDto BuildPutLabelPayload(int labelId, int boardId) 
        => new()
        {
            Id = labelId,
            Name = "Updated Label",
            Colour = Colour.Yellow,
            BoardId = boardId
        };
}
