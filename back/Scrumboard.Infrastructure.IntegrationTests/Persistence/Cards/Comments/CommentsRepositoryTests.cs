using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.Infrastructure.Persistence.Cards.Comments;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards.Comments;

public sealed class CommentsRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly ICommentsRepository _sut;
    
    public CommentsRepositoryTests(DatabaseFixture databaseFixture)
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        _sut = new CommentsRepository(ActDbContext);
    }
    
    [Fact]
    public async Task Should_add_Comment()
    {
        // Arrange
        var card = await Given_a_Card();
        
        var comment = _fixture.Create<Comment>();
        comment.SetProperty(x => x.CardId, card.Id);
        
        // Act
        await _sut.AddAsync(comment);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var createdComment = await AssertDbContext.Comments
            .FirstAsync(x => x.Id == comment.Id);

        createdComment.Should()
            .BeEquivalentTo(comment);
    }

    [Fact]
    public async Task Should_delete_Comment()
    {
        // Arrange
        var comment = await Given_a_Comment();

        // Act
        await _sut.DeleteAsync(comment.Id);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var commentExists = await AssertDbContext.Comments
            .AnyAsync(x => x.Id == comment.Id);

        commentExists.Should()
            .BeFalse();
    }

    [Fact]
    public async Task Should_get_Comment_by_Id()
    {
        // Arrange
        var existingComment = await Given_a_Comment();

        // Act
        var comment = await _sut.TryGetByIdAsync(existingComment.Id);

        // Assert
        comment.Should()
            .BeEquivalentTo(existingComment);
    }

    [Fact]
    public async Task Should_update_Comment()
    {
        // Arrange
        var comment = await Given_a_Comment_for_edition();
        comment.SetProperty(x => x.Message, _fixture.Create<string>());

        // Act
        _sut.Update(comment);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var updatedComment = await AssertDbContext.Comments
            .FirstAsync(x => x.Id == comment.Id);

        updatedComment.Should()
            .BeEquivalentTo(comment);
    }

    private async Task<Comment> Given_a_Comment_for_edition()
    {
        var comment = await Given_a_Comment();
        
        return await ActDbContext.Comments
            .FirstAsync(x => x.Id == comment.Id);
    }
    
    private async Task<Comment> Given_a_Comment()
    {
        var card = await Given_a_Card();
        
        var comment = _fixture.Create<Comment>();
        comment.SetProperty(x => x.CardId, card.Id);
        
        await ArrangeDbContext.Comments.AddAsync(comment);
        await ArrangeDbContext.SaveChangesAsync();

        return comment;
    }
    
    private async Task<Card> Given_a_Card()
    {
        var listBoard = await Given_a_ListBoard();
        var label = await Given_a_Label();
        
        var card = new Card(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            dueDate: _fixture.Create<DateTimeOffset>(),
            position: _fixture.Create<int>(),
            listBoardId: listBoard.Id,
            assigneeIds: _fixture.CreateMany<AssigneeId>().ToArray(),
            labelIds: [label.Id]);

        await ArrangeDbContext.Cards.AddAsync(card);

        await ArrangeDbContext.SaveChangesAsync();

        return card;
    }
    
    private async Task<ListBoard> Given_a_ListBoard()
    {
        var board = await Given_a_Board();
        
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.BoardId, board.Id);
        
        await ArrangeDbContext.ListBoards.AddAsync(listBoard);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return listBoard;
    }
    
    private async Task<Label> Given_a_Label()
    {
        var board = await Given_a_Board();
        
        var label = _fixture.Create<Label>();
        label.SetProperty(x => x.BoardId, board.Id);

        await ArrangeDbContext.Labels.AddAsync(label);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return label;
    }
    
    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        await ArrangeDbContext.Boards.AddAsync(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
