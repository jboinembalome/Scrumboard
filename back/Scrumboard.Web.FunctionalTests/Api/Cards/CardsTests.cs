using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Web.Api.Cards;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Cards;

public sealed class CardsTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_card_by_id_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        var card = await Given_a_Card(SeededUser.Adherent.Id);

        // Act
        var response = await client.GetAsync($"/api/cards/{card.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_card_by_id_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        var card = await Given_a_Card(SeededUser.NoRightUser.Id);

        // Act
        var response = await client.GetAsync($"/api/cards/{card.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Post_card_should_return_Created()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        var listBoard = await Given_a_ListBoard(SeededUser.Adherent.Id);
        
        var payload = BuildPostCardPayload(listBoard.Id.Value);

        // Act
        var response = await client.PostAsJsonAsync("/api/cards", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_card_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        var listBoard = await Given_a_ListBoard(SeededUser.NoRightUser.Id);
        
        var payload = BuildPostCardPayload(listBoard.Id.Value);

        // Act
        var response = await client.PostAsJsonAsync("/api/cards", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Put_card_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        var card = await Given_a_Card(SeededUser.Adherent.Id);
        
        var payload = BuildPutCardPayload(card.Id.Value, card.ListBoardId.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/cards/{card.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_card_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        var card = await Given_a_Card(SeededUser.NoRightUser.Id);
        
        var payload = BuildPutCardPayload(card.Id.Value, card.ListBoardId.Value);

        // Act
        var response = await client.PutAsJsonAsync($"/api/cards/{card.Id}", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_card_should_return_NoContent()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        var card = await Given_a_Card(SeededUser.Adherent.Id);

        // Act
        var response = await client.DeleteAsync($"/api/cards/{card.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_card_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        var card = await Given_a_Card(SeededUser.NoRightUser.Id);

        // Act
        var response = await client.DeleteAsync($"/api/cards/{card.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    private async Task<Card> Given_a_Card(OwnerId ownerId)
    {
        var listBoard = await Given_a_ListBoard(ownerId);
        var label = await Given_a_Label(listBoard.BoardId);
        
        var card = new Card(
            name: "Test Card",
            description: "Description",
            dueDate: DateTimeOffset.Now.AddDays(1),
            position: 1,
            listBoardId: listBoard.Id,
            assigneeIds: [new AssigneeId(ownerId.Value)],
            labelIds: [label.Id]);

        return await _factory.AddEntityAsync(card);
    }

    private async Task<ListBoard> Given_a_ListBoard(OwnerId ownerId)
    {
        var board = await Given_a_Board(ownerId);
        
        return await _factory.AddEntityAsync(new ListBoard(
            name: "Test ListBoard",
            position: 1,
            boardId: board.Id));
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
            colour: Colour.Blue,
            boardId: boardId));
    
    private static CardCreationDto BuildPostCardPayload(int listBoardId) 
        => new()
        {
            Name = "New Card",
            Description = "Card Description",
            DueDate = DateTimeOffset.Now.AddDays(3),
            ListBoardId = listBoardId
        };

    private static CardEditionDto BuildPutCardPayload(int cardId, int listBoardId) 
        => new()
        {
            Id = cardId,
            Name = "Updated Card",
            Description = "Updated Description",
            DueDate = DateTimeOffset.Now.AddDays(5),
            Position = 1,
            ListBoardId = listBoardId
        };
}
