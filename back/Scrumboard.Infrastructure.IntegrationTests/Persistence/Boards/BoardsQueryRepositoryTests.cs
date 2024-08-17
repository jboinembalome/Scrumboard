using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class BoardsQueryRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly IBoardsQueryRepository _sut;
    
    public BoardsQueryRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        _sut = new BoardsQueryRepository(ActDbContext);
    }
    
    [Fact]
    public async Task Should_get_board_by_id()
    {           
        // Arrange
        var existingBoard = await Given_a_board();
        
        // Act
        var board = await _sut.TryGetByIdAsync(existingBoard.Id);
    
        // Assert
        board.Should()
            .NotBeNull();

        board!.Id.Should()
            .Be(existingBoard.Id);
    }
    
    [Fact]
    public async Task Should_get_board_by_owner_id()
    {           
        // Arrange
        var firstOwnerId = _fixture.Create<OwnerId>();
        var firstBoard = await Given_a_board(firstOwnerId);
        
        var secondOwnerId = _fixture.Create<OwnerId>();
        await Given_a_board(secondOwnerId);
        
        // Act
        var boards = await _sut.GetByOwnerIdAsync(firstOwnerId);
    
        // Assert
        boards.Should()
            .OnlyContain(x => x.Id == firstBoard.Id 
                              && x.OwnerId == firstBoard.OwnerId);
    }
    
    private async Task<Board> Given_a_board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
    
    private async Task<Board> Given_a_board(OwnerId ownerId)
    {
        var board = _fixture.Create<Board>();
        board.SetProperty(x => x.OwnerId, ownerId);
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
