using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.ListBoards;

public sealed class ListBoardsQueryRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    
    private readonly IListBoardsQueryRepository _sut;
    
    public ListBoardsQueryRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        _sut = new ListBoardsQueryRepository(ActDbContext);
    }
    
    [Fact]
    public async Task Should_get_ListBoards_by_BoardId_without_including_Cards()
    {
        // Arrange
        var listBoard = await Given_a_ListBoard_with_Cards();

        // Act
        var listBoards = await _sut.GetByBoardIdAsync(listBoard.BoardId, includeCards: false);

        // Assert
        listBoards.Should()
            .ContainSingle()
            .Which.Should()
                .BeEquivalentTo(listBoard, options => options.Excluding(x => x.Cards));
    }

    [Fact]
    public async Task Should_get_ListBoards_by_BoardId_with_including_Cards()
    {
        // Arrange
        var listBoard = await Given_a_ListBoard_with_Cards();

        // Act
        var listBoards = await _sut.GetByBoardIdAsync(listBoard.BoardId, includeCards: true);

        // Assert
        listBoards.Should()
            .ContainSingle()
            .Which.Should()
                .BeEquivalentTo(listBoard);
    }

    [Fact]
    public async Task Should_get_ListBoard_by_Id()
    {
        // Arrange
        var existingListBoard = await Given_a_ListBoard();

        // Act
        var listBoard = await _sut.TryGetByIdAsync(existingListBoard.Id);

        // Assert
        listBoard.Should()
            .BeEquivalentTo(existingListBoard);
    }

    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        await ArrangeDbContext.Boards.AddAsync(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
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

    private async Task<ListBoard> Given_a_ListBoard_with_Cards()
    {
        var listBoard = await Given_a_ListBoard();
        var cards = await Given_some_cards(listBoard);
        
        listBoard.SetProperty(x => x.Cards, cards);
        
        return listBoard;
    }
    
    private async Task<IReadOnlyCollection<Card>> Given_some_cards(ListBoard listBoard)
    {
        var labels = await Given_some_labels(listBoard.BoardId);
        var cards = _fixture.CreateMany<Card>()
            .ToList();
        
        cards.ForEach(card =>
        {
            card.SetProperty(x => x.ListBoardId, listBoard.Id);
            
            var assigneeId = _fixture.Create<AssigneeId>();
            card.AddAssignees([assigneeId]);
            
            card.AddLabels(labels.Select(x => x.Id));
        });
        
        await ArrangeDbContext.Cards.AddRangeAsync(cards);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return cards;
    }
    
    private async Task<IReadOnlyCollection<Label>> Given_some_labels(BoardId boardId)
    {
        var labels = _fixture.CreateMany<Label>()
            .ToList();
        
        labels.ForEach(label => label
            .SetProperty(x => x.BoardId, boardId));
        
        await ArrangeDbContext.Labels.AddRangeAsync(labels);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return labels;
    }
}
