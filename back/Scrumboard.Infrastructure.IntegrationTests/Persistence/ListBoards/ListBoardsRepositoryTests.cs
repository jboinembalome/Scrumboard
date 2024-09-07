using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.ListBoards;

public sealed class ListBoardsRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly IListBoardsRepository _sut;
    
    public ListBoardsRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        _sut = new ListBoardsRepository(ActDbContext);
    }
    
    [Fact]
    public async Task Should_add_ListBoard()
    {
        // Arrange
        var board = await Given_a_Board();
        
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.BoardId, board.Id);

        // Act
        await _sut.AddAsync(listBoard);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var createdListBoard = await AssertDbContext.ListBoards
            .FirstAsync(x => x.Id == listBoard.Id);

        createdListBoard.Should()
            .BeEquivalentTo(listBoard);
    }

    [Fact]
    public async Task Should_delete_ListBoard()
    {
        // Arrange
        var listBoard = await Given_a_ListBoard();

        // Act
        await _sut.DeleteAsync(listBoard.Id);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var listBoardExist = await AssertDbContext.ListBoards
            .AnyAsync(x => x.Id == listBoard.Id);

        listBoardExist.Should()
            .BeFalse();
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

    [Fact]
    public async Task Should_update_ListBoard()
    {
        // Arrange
        var listBoard = await Given_a_ListBoard_for_edition();
        listBoard.SetProperty(x => x.Name, _fixture.Create<string>());
        listBoard.SetProperty(x => x.Position, _fixture.Create<int>());

        // Act
        _sut.Update(listBoard);
        await ActDbContext.SaveChangesAsync();

        // Assert
        var updatedListBoard = await AssertDbContext.ListBoards
            .FirstAsync(x => x.Id == listBoard.Id);

        updatedListBoard.Should()
            .BeEquivalentTo(listBoard);
    }

    private async Task<ListBoard> Given_a_ListBoard()
    {
        var board = await Given_a_Board();
        
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.BoardId, board.Id);

        ArrangeDbContext.ListBoards.Add(listBoard);
        await ArrangeDbContext.SaveChangesAsync();

        return listBoard;
    }

    private async Task<ListBoard> Given_a_ListBoard_for_edition()
    {
        var listBoard = await Given_a_ListBoard();
        
        return await ActDbContext.ListBoards
            .FirstAsync(x => x.Id == listBoard.Id);
    }
    
    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
