using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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

public sealed class CardsRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    
    private readonly ICardsRepository _sut;

    public CardsRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        _sut = new CardsRepository(ActDbContext);
    }

    [Fact]
    public async Task Should_add_Card()
    {
        // Arrange
        var listBoard = await Given_a_ListBoard();
        
        var card = _fixture.Create<Card>();
        card.SetProperty(x => x.ListBoardId, listBoard.Id);

        // Act
        await _sut.AddAsync(card);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var createdCard = await AssertDbContext.Cards
            .Include(x => x.Assignees)
            .Include(x => x.Labels)
            .FirstAsync(x => x.Id == card.Id);

        createdCard.Should()
            .BeEquivalentTo(card);
    }

    [Fact]
    public async Task Should_delete_Card()
    {
        // Arrange
        var card = await Given_a_Card();

        // Act
        await _sut.DeleteAsync(card.Id);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var cardExists = await AssertDbContext.Cards
            .AnyAsync(x => x.Id == card.Id);

        cardExists.Should()
            .BeFalse();
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
    public async Task Should_update_Card()
    {
        // Arrange
        var card = await Given_a_Card_for_edition();
        
        card.Update(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            dueDate: _fixture.Create<DateTimeOffset>(),
            position: _fixture.Create<int>(),
            listBoardId: card.ListBoardId,
            assigneeIds: [],
            labelIds: []);

        // Act
        _sut.Update(card);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var updatedCard = await AssertDbContext.Cards
            .Include(x => x.Assignees)
            .Include(x => x.Labels)
            .FirstAsync(x => x.Id == card.Id);

        updatedCard.Should()
            .BeEquivalentTo(card, opt => opt
                .Excluding(x => x.DomainEvents));
    }

    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
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

        ArrangeDbContext.Cards.Add(card);

        await ArrangeDbContext.SaveChangesAsync();

        return card;
    }

    private async Task<Card> Given_a_Card_for_edition()
    {
        var card = await Given_a_Card();

        return await ActDbContext.Cards
            .Include(x => x.Assignees)
            .Include(x => x.Labels)
            .FirstAsync(x => x.Id == card.Id);
    }
}

