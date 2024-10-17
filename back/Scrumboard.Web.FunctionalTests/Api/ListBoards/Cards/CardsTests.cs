using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.ListBoards.Cards;

public sealed class CardsTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_cards_by_listBoardId_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);
        
        var listBoard = await Given_a_ListBoard(SeededUser.Adherent.Id);
        await Given_a_Card(listBoard);

        // Act
        var response = await client.GetAsync($"/api/list-boards/{listBoard.Id}/cards");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_cards_by_listBoardId_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var listBoard = await Given_a_ListBoard(SeededUser.NoRightUser.Id);
        
        // Act
        var response = await client.GetAsync($"/api/list-boards/{listBoard.Id}/cards");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    private async Task<Card> Given_a_Card(ListBoard listBoard)
    {
        var label = await Given_a_Label(listBoard.BoardId);
        
        var card = new Card(
            name: "Test Card",
            description: "Description",
            dueDate: DateTimeOffset.Now.AddDays(1),
            position: 1,
            listBoardId: listBoard.Id,
            assigneeIds: [],
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
}
