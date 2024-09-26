using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Cards;

public sealed class CardsQueryRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    
    private readonly ICardsQueryRepository _sut;

    public CardsQueryRepositoryTests(DatabaseFixture databaseFixture)
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        _sut = new CardsQueryRepository(ActDbContext);
    }
    
    [Fact]
    public async Task Should_get_Card_by_Id()
    {
        // Arrange
        var existingCard = await Given_a_Card();

        // Act
        var card = await _sut.TryGetByIdAsync(existingCard.Id);

        // Assert
        card.Should()
            .BeEquivalentTo(existingCard);
    }

    [Fact]
    public async Task Should_return_null_when_Card_does_not_exist()
    {
        // Arrange
        var nonExistentCardId = new CardId(999);

        // Act
        var card = await _sut.TryGetByIdAsync(nonExistentCardId);

        // Assert
        card.Should()
            .BeNull();
    }

    [Fact]
    public async Task Should_get_Cards_by_ListBoardId()
    {
        // Arrange
        var listBoard = await Given_a_ListBoard();
        var cards = new List<Card>
        {
            await Given_a_Card(listBoard.Id),
            await Given_a_Card(listBoard.Id)
        };

        // Act
        var result = await _sut.GetByListBoardIdAsync(listBoard.Id);

        // Assert
        result.Should()
            .BeEquivalentTo(cards);
    }

    [Fact]
    public async Task Should_return_empty_list_when_no_Cards_for_ListBoardId()
    {
        // Arrange
        var nonexistentListBoardId = new ListBoardId(999);

        // Act
        var result = await _sut.GetByListBoardIdAsync(nonexistentListBoardId);

        // Assert
        result.Should()
            .BeEmpty();
    }
    
    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        await ArrangeDbContext.Boards.AddAsync(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
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
    
    private async Task<ListBoard> Given_a_ListBoard()
    {
        var board = await Given_a_Board();
        
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.BoardId, board.Id);
        
        await ArrangeDbContext.ListBoards.AddAsync(listBoard);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return listBoard;
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
    
    private async Task<Card> Given_a_Card(ListBoardId listBoardId)
    {
        var label = await Given_a_Label();
        
        var card = new Card(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            dueDate: _fixture.Create<DateTimeOffset>(),
            position: _fixture.Create<int>(),
            listBoardId: listBoardId,
            assigneeIds: _fixture.CreateMany<AssigneeId>().ToArray(),
            labelIds: [label.Id]);

        await ArrangeDbContext.Cards.AddAsync(card);

        await ArrangeDbContext.SaveChangesAsync();

        return card;
    }
}
