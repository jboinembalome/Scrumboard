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
        var createdBoardDao = await AssertDbContext.Boards
            .Include(x => x.BoardSetting)
            .FirstAsync(x => x.Name == boardCreation.Name);

        createdBoardDao.Id.Should().BeGreaterThan(0);
        createdBoardDao.Name.Should().Be(boardCreation.Name);
        createdBoardDao.IsPinned.Should().Be(boardCreation.IsPinned);

        createdBoardDao.BoardSetting.Should().NotBeNull();
        createdBoardDao.BoardSetting.Id.Should().BeGreaterThan(0);
        createdBoardDao.BoardSetting.BoardId.Should().Be(createdBoardDao.Id);
        createdBoardDao.BoardSetting.Colour.Should().Be(boardCreation.BoardSetting.Colour);
    }
    
    [Fact]
    public async Task Should_delete_board()
    {           
        // Arrange
        var boardDao = await Given_a_board();
        
        // Act
        await _sut.DeleteAsync((BoardId)boardDao.Id);
    
        // Assert
        var boardExist = await AssertDbContext.Boards
            .AnyAsync(x => x.Id == boardDao.Id);
        
        boardExist.Should()
            .BeFalse();
    }
    
    [Fact]
    public async Task Should_get_board_by_id()
    {           
        // Arrange
        var boardDao = await Given_a_board();
        var boardId = (BoardId)boardDao.Id;
        
        // Act
        var board = await _sut.TryGetByIdAsync(boardId);
    
        // Assert
        board.Should()
            .NotBeNull();

        board!.Id.Should()
            .Be(boardId);
    }
    
    
    [Fact]
    public async Task Should_update_board()
    {           
        // Arrange
        var boardDao = await Given_a_board();
        var boardId = (BoardId)boardDao.Id;
        
        var boardEdition = _fixture.Create<BoardEdition>();
        boardEdition.Id = boardId;
        
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
    
    private async Task<BoardDao> Given_a_board()
    {
        var boardDao = BuildBoardDao();
        
        ArrangeDbContext.Boards.Add(boardDao);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return boardDao;
    }

    private BoardDao BuildBoardDao()
    {
        var boardDao = _fixture.Build<BoardDao>()
            .Without(x => x.Id)
            .Without(x => x.ListBoards)
            .Create();
        
        var teamMemberDao = BuildTeamMemberDao();
        boardDao.Team.Members = [teamMemberDao];
        
        return boardDao;
    }

    private TeamMemberDao BuildTeamMemberDao() 
        => _fixture.Build<TeamMemberDao>()
            .Without(x => x.TeamId)
            .With(x => x.MemberId, Guid.NewGuid().ToString())
            .Create();
}
