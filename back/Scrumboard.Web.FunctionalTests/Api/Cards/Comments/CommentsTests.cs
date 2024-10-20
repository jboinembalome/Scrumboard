using System.Net;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Web.Api.Cards.Comments;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Cards.Comments;

public sealed class CommentsTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Fact]
    public async Task Get_comments_by_cardId_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        var card = await Given_a_Card(SeededUser.Adherent.Id);
        await Given_a_Comment(card.Id);

        // Act
        var response = await client.GetAsync($"/api/cards/{card.Id}/comments");

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_comments_by_cardId_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var card = await Given_a_Card(SeededUser.Adherent.Id);
        
        // Act
        var response = await client.GetAsync($"/api/cards/{card.Id}/comments");

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_comment_by_id_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var comment = await Given_a_Comment(card.Id);

        // Act
        var response = await client.GetAsync($"/api/cards/{card.Id}/comments/{comment.Id}");

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_comment_by_id_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var comment = await Given_a_Comment(card.Id);

        // Act
        var response = await client.GetAsync($"/api/cards/{card.Id}/comments/{comment.Id}");

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Post_comment_should_return_Created()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var payload = BuildCreateCommentPayload(card.Id);

        // Act
        var response = await client.PostAsJsonAsync($"/api/cards/{card.Id}/comments", payload);

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Post_comment_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var payload = BuildCreateCommentPayload(card.Id);

        // Act
        var response = await client.PostAsJsonAsync($"/api/cards/{card.Id}/comments", payload);

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_comment_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var comment = await Given_a_Comment(card.Id);

        var payload = BuildUpdateCommentPayload(comment.Id, card.Id);

        // Act
        var response = await client.PutAsJsonAsync($"/api/cards/{card.Id}/comments/{comment.Id}", payload);

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Put_comment_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var comment = await Given_a_Comment(card.Id);

        var payload = BuildUpdateCommentPayload(comment.Id, card.Id);

        // Act
        var response = await client.PutAsJsonAsync($"/api/cards/{card.Id}/comments/{comment.Id}", payload);

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_comment_should_return_NoContent()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var comment = await Given_a_Comment(card.Id);

        // Act
        var response = await client.DeleteAsync($"/api/cards/{card.Id}/comments/{comment.Id}");

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Delete_comment_should_return_Forbidden_when_user_has_no_right()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.NoRightUser);
        
        var card = await Given_a_Card(SeededUser.Adherent.Id);
        var comment = await Given_a_Comment(card.Id);

        // Act
        var response = await client.DeleteAsync($"/api/cards/{card.Id}/comments/{comment.Id}");

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.Forbidden);
    }
    
    private Task<Comment> Given_a_Comment(CardId cardId)
        => _factory.AddEntityAsync(new Comment(
            message: "Test comment",
            cardId: cardId));
    
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

    private static CommentCreationDto BuildCreateCommentPayload(CardId cardId)
        => new()
        {
            Message = "New Comment",
            CardId = cardId.Value
        };

    private static CommentEditionDto BuildUpdateCommentPayload(
        CommentId commentId,
        CardId cardId)
        => new()
        {
            Id = commentId.Value,
            Message = "Updated Comment",
            CardId = cardId.Value
        };

}
