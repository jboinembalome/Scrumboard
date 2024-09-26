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
    public async Task Should_get_Board_by_Id()
    {           
        // Arrange
        var existingBoard = await Given_a_Board();
        
        // Act
        var board = await _sut.TryGetByIdAsync(existingBoard.Id);
    
        // Assert
        board.Should()
            .NotBeNull();

        board.Should()
            .BeEquivalentTo(existingBoard);
    }
    
    [Fact]
    public async Task Should_get_Boards_by_OwnerId()
    {           
        // Arrange
        var firstOwnerId = _fixture.Create<OwnerId>();
        var firstBoard = await Given_a_Board(firstOwnerId);
        
        var secondOwnerId = _fixture.Create<OwnerId>();
        await Given_a_Board(secondOwnerId);
        
        // Act
        var boards = await _sut.GetByOwnerIdAsync(firstOwnerId);
    
        // Assert
        boards.Should()
            .BeEquivalentTo([firstBoard]);
    }
    
    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
    
    private async Task<Board> Given_a_Board(OwnerId ownerId)
    {
        var board = _fixture.Create<Board>();
        board.SetProperty(x => x.OwnerId, ownerId);
        
        await ArrangeDbContext.Boards.AddAsync(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
