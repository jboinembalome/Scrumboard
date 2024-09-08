using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Boards.ListBoards;

public sealed class ListBoardsTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_listBoards_by_boardId_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        var listBoard = await Given_a_ListBoard(SeededUser.Adherent.Id);
        await Given_a_Card(listBoard, SeededUser.Adherent.Id);

        // Act
        var response = await client.GetAsync($"/api/boards/{listBoard.BoardId}/list-boards?includeCards=true");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_listBoards_by_boardId_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var board = await Given_a_Board(SeededUser.NoRightUser.Id);
        
        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}/list-boards");

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
    
    private async Task<Card> Given_a_Card(ListBoard listBoard, AssigneeId assigneeId)
    {
        var label = await Given_a_Label(listBoard.BoardId);
        
        var card = new Card
        {
            Name = "Test Card",
            Description = "Description",
            ListBoardId = listBoard.Id
        };
        
        card.AddLabels([label.Id]);
        card.AddAssignees([assigneeId]);
        return await _factory.AddEntityAsync(card);
    }
    
    private Task<Label> Given_a_Label(BoardId boardId)
        => _factory.AddEntityAsync(new Label(
            name: "Test Label",
            colour: Colour.Blue,
            boardId: boardId));
}
