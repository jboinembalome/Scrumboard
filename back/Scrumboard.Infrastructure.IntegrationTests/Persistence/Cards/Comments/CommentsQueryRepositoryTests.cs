using AutoFixture;
using FluentAssertions;
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

public sealed class CommentsQueryRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly ICommentsQueryRepository _sut;
    
    public CommentsQueryRepositoryTests(DatabaseFixture databaseFixture)
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        _sut = new CommentsQueryRepository(ActDbContext);
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
    public async Task Should_get_Comments_by_CardId()
    {
        // Arrange
        var card = await Given_a_Card();
        var comments = await Given_somme_Comments(card.Id);

        // Act
        var result = await _sut.GetByCardIdAsync(card.Id);

        // Assert
        result.Should()
            .BeEquivalentTo(comments);
    }

    private async Task<IReadOnlyCollection<Comment>> Given_somme_Comments(CardId cardId)
    {
        var comments = _fixture.CreateMany<Comment>()
            .ToArray();

        foreach (var comment in comments)
        {
            comment.SetProperty(x => x.CardId, cardId);
        }
        
        await ArrangeDbContext.Comments.AddRangeAsync(comments);
        await ArrangeDbContext.SaveChangesAsync();

        return comments;
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
