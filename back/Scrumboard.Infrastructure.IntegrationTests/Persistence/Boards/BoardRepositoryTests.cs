using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

public sealed class BoardRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly IBoardsRepository _sut;
    
    public BoardRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BoardProfile>();
            cfg.AddProfile<TeamProfile>();
        });
        
        var mapper = mapperConfiguration.CreateMapper();
        
        _sut = new BoardsRepository(ActDbContext, mapper);
    }

    [Fact]
    public async Task Should_add_board()
    {           
        // Arrange
        var boardCreation = _fixture.Create<BoardCreation>();

        // Act
        await _sut.AddAsync(boardCreation);
        
        // Assert
        var createdBoard = await AssertDbContext.Boards
            .Include(x => x.BoardSetting)
            .FirstAsync(x => x.Name == boardCreation.Name);

        createdBoard.Id.Value.Should().BeGreaterThan(0);
        createdBoard.Name.Should().Be(boardCreation.Name);
        createdBoard.IsPinned.Should().Be(boardCreation.IsPinned);

        createdBoard.BoardSetting.Should().NotBeNull();
        createdBoard.BoardSetting.Id.Value.Should().BeGreaterThan(0);
        createdBoard.BoardSetting.BoardId.Should().Be(createdBoard.Id);
        createdBoard.BoardSetting.Colour.Should().Be(boardCreation.BoardSetting.Colour);
    }
    
    [Fact]
    public async Task Should_delete_board()
    {           
        // Arrange
        var board = await Given_a_board();
        
        // Act
        await _sut.DeleteAsync(board.Id);
    
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
        var existingBoard = await Given_a_board();
        
        var boardEdition = _fixture.Create<BoardEdition>();
        boardEdition.Id = existingBoard.Id;
        
        // Act
        await _sut.UpdateAsync(boardEdition);
    
        // Assert
        var updatedBoardDao = await AssertDbContext.Boards
            .Include(x => x.BoardSetting)
            .FirstAsync(x => x.Id == boardEdition.Id);
        
        updatedBoardDao.Id.Should().Be(boardEdition.Id);
        updatedBoardDao.Name.Should().Be(boardEdition.Name);
        updatedBoardDao.IsPinned.Should().Be(boardEdition.IsPinned);

        updatedBoardDao.BoardSetting.Should().NotBeNull();
        updatedBoardDao.BoardSetting.BoardId.Should().Be(updatedBoardDao.BoardSetting.BoardId);
        updatedBoardDao.BoardSetting.Colour.Should().Be(boardEdition.BoardSetting.Colour);
    }
    
    private async Task<Board> Given_a_board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
