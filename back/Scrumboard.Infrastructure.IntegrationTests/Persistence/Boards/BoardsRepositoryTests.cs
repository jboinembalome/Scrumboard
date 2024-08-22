using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class BoardsRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly IBoardsRepository _sut;
    
    public BoardsRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        
        _sut = new BoardsRepository(ActDbContext);
    }

    [Fact]
    public async Task Should_add_board()
    {           
        // Arrange
        var board = _fixture.Create<Board>();

        // Act
        await _sut.AddAsync(board);
        await ActDbContext.SaveChangesAsync();
        
        // Assert
        var createdBoard = await AssertDbContext.Boards
            .Include(x => x.BoardSetting)
            .FirstAsync(x => x.Name == board.Name);

        createdBoard.Id.Value.Should().BeGreaterThan(0);
        createdBoard.Name.Should().Be(board.Name);
        createdBoard.IsPinned.Should().Be(board.IsPinned);
        createdBoard.OwnerId.Should().Be(board.OwnerId);
        createdBoard.BoardSetting.Should().NotBeNull();
        createdBoard.BoardSetting.Id.Value.Should().BeGreaterThan(0);
        createdBoard.BoardSetting.BoardId.Should().Be(createdBoard.Id);
        createdBoard.BoardSetting.Colour.Should().Be(board.BoardSetting.Colour);
    }
    
    [Fact]
    public async Task Should_delete_board()
    {           
        // Arrange
        var board = await Given_a_board();
        
        // Act
        await _sut.DeleteAsync(board.Id);
        await ActDbContext.SaveChangesAsync();
    
        // Assert
        var boardExist = await AssertDbContext.Boards
            .AnyAsync(x => x.Id == board.Id);
        
        boardExist.Should()
            .BeFalse();
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
    public async Task Should_update_board()
    {           
        // Arrange
        var board = await Given_a_board();
        board.SetProperty(x => x.Name, _fixture.Create<string>());
        board.SetProperty(x => x.IsPinned, _fixture.Create<bool>());
        board.SetProperty(x => x.OwnerId, _fixture.Create<OwnerId>());
        board.BoardSetting.SetProperty(x => x.Colour, _fixture.Create<Colour>());
        
        // Act
        _sut.Update(board);
        await ActDbContext.SaveChangesAsync();
        
        // Assert
        var updatedBoard = await AssertDbContext.Boards
            .Include(x => x.BoardSetting)
            .FirstAsync(x => x.Id == board.Id);
        
        updatedBoard.Name.Should().Be(board.Name);
        updatedBoard.IsPinned.Should().Be(board.IsPinned);
        updatedBoard.OwnerId.Should().Be(board.OwnerId);
        updatedBoard.BoardSetting.Colour.Should().Be(board.BoardSetting.Colour);
    }
    
    private async Task<Board> Given_a_board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
