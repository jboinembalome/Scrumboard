using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Teams;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

[Collection("Database collection")]
public class BoardRepositoryTests : IAsyncLifetime
{
    private readonly DatabaseFixture _database;

    public BoardRepositoryTests(DatabaseFixture database)
    {
        _database = database;
    }

    public async Task DisposeAsync() =>  await _database.ResetState();

    public Task InitializeAsync() => Task.CompletedTask;
    
    [Fact]
    public async Task AddAsync_ValidBoard_BoardAdded()
    {           
        // Arrange
        var testBoardName = "testBoard";
        var board = new Board
        {
            Name = testBoardName,
            BoardSetting = new BoardSetting
            {
                Colour = Colour.Gray
            },
            Team = new Team
            {
                Name = "Team 1"
            }
        };
        var boardRepository =  _database.GetRepository<Board, int>();

        // Act
        var newboard = await boardRepository.AddAsync(board);
        var boardAdded = (await _database.DbContext.Boards.ToListAsync())[0];

        // Assert
        boardAdded.Name.Should().Be(newboard.Name);
        boardAdded.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task DeleteAsync_ExistingBoard_BoardDeleted()
    {           
        // Arrange
        var testBoardName = "testBoard";
        var board = new Board
        {
            Name = testBoardName,
            BoardSetting = new BoardSetting
            {
                Colour = Colour.Gray
            },
            Team = new Team
            {
                Name = "Team 1"
            }
        };
        var boardRepository =  _database.GetRepository<Board, int>();

        _database.DbContext.Boards.Add(board);
        await _database.DbContext.SaveChangesAsync();

        // Act
        await boardRepository.DeleteAsync(board);
        var boardDeleted = await _database.DbContext.Boards.FirstOrDefaultAsync(b => b.Name == testBoardName);

        // Assert
        boardDeleted.Should().BeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_ExistingId_BoardRetrieved()
    {           
        // Arrange
        var testBoardName = "testBoard";
        
        var board = new Board
        {
            Name = testBoardName,
            BoardSetting = new BoardSetting
            {
                Colour = Colour.Gray
            },
            Team = new Team
            {
                Name = "Team 1"
            }
        };
        var boardRepository =  _database.GetRepository<Board, int>();

        _database.DbContext.Boards.Add(board);
        await _database.DbContext.SaveChangesAsync();

        int boardId = board.Id;

        // Act
        var boardRetrived = await boardRepository.GetByIdAsync(boardId);

        // Assert
        boardRetrived!.Name.Should().Be(board.Name);
        boardRetrived.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task UpdateAsync_ExistingBoard_BoardUpdated()
    {           
        // Arrange
        var testBoardName = "testBoard";
        var board = new Board
        {
            Name = testBoardName,
            BoardSetting = new BoardSetting
            {
                Colour = Colour.Gray
            },
            Team = new Team
            {
                Name = "Team 1"
            }
        };
        var boardRepository =  _database.GetRepository<Board, int>();

        _database.DbContext.Boards.Add(board);
        await _database.DbContext.SaveChangesAsync();

        board.Name = "Updated Name";

        // Act
        await boardRepository.UpdateAsync(board);
        var boardUpdated = await _database.DbContext.Boards.FirstOrDefaultAsync(b => b.Name == board.Name);

        // Assert
        boardUpdated.Should().NotBeNull();
        boardUpdated!.Id.Should().Be(board.Id);
        boardUpdated.Name.Should().Be(board.Name);        
    }
}
