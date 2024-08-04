using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.ListBoards;
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
        _fixture = new Fixture().Customize(new UserIdCustomization());
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BoardProfile>();
            cfg.AddProfile<TeamProfile>();
        });
        
        var mapper = mapperConfiguration.CreateMapper();
        
        _sut = new BoardsRepository(databaseFixture.DbContext, mapper);
    }

    [Fact]
    public async Task Should_add_board()
    {           
        // Arrange
        var boardCreation = _fixture.Create<BoardCreation>();

        // Act
        var board = await _sut.AddAsync(boardCreation);
        
        // Assert
        board.Id.Value
            .Should()
            .BeGreaterThan(0);
    }
    
    [Fact]
    public async Task Should_delete_board()
    {           
        // Arrange
        var boardDao = await Given_a_board();
        
        // Act
        await _sut.DeleteAsync((BoardId)boardDao.Id);
        
        var boardExist = await DbContext.Boards.AnyAsync(x => x.Id == boardDao.Id);
    
        // Assert
        boardExist
            .Should()
            .BeFalse();
    }
    
    //
    // [Fact]
    // public async Task GetByIdAsync_ExistingId_BoardRetrieved()
    // {           
    //     // Arrange
    //     var testBoardName = "testBoard";
    //     
    //     var board = new BoardDao
    //     {
    //         Name = testBoardName,
    //         BoardSetting = new BoardSettingDao
    //         {
    //             Colour = Colour.Gray
    //         },
    //         Team = new TeamDao
    //         {
    //             Name = "Team 1"
    //         }
    //     };
    //     var boardRepository =  database.GetRepository<Board, int>();
    //
    //     database.DbContext.Boards.Add(board);
    //     await database.DbContext.SaveChangesAsync();
    //
    //     int boardId = board.Id;
    //
    //     // Act
    //     var boardRetrived = await boardRepository.TryGetByIdAsync(boardId);
    //
    //     // Assert
    //     boardRetrived!.Name.Should().Be(board.Name);
    //     boardRetrived.Id.Should().BeGreaterThan(0);
    // }
    //
    // [Fact]
    // public async Task UpdateAsync_ExistingBoard_BoardUpdated()
    // {           
    //     // Arrange
    //     var testBoardName = "testBoard";
    //     var boardDao = new BoardDao
    //     {
    //         Name = testBoardName,
    //         BoardSetting = new BoardSettingDao
    //         {
    //             Colour = Colour.Gray
    //         },
    //         Team = new TeamDao
    //         {
    //             Name = "Team 1"
    //         }
    //     };
    //     var boardRepository =  database.GetRepository<Board, int>();
    //
    //     database.DbContext.Boards.Add(boardDao);
    //     await database.DbContext.SaveChangesAsync();
    //
    //     boardDao.Name = "Updated Name";
    //
    //     var board = BuildBoard(boardDao);
    //     
    //     // Act
    //     await boardRepository.UpdateAsync(board);
    //     var boardUpdated = await database.DbContext.Boards.FirstOrDefaultAsync(b => b.Name == boardDao.Name);
    //
    //     // Assert
    //     boardUpdated.Should().NotBeNull();
    //     boardUpdated!.Id.Should().Be(boardDao.Id);
    //     boardUpdated.Name.Should().Be(boardDao.Name);        
    // }
    //
    // private static Board BuildBoard(BoardDao dao)
    // {
    //     return new Board
    //     {
    //         Id = dao.Id,
    //         Name = dao.Name,
    //         Uri = dao.Uri,
    //         IsPinned = dao.IsPinned,
    //         BoardSetting = new BoardSetting
    //         {
    //             Id = dao.BoardSetting.Id,
    //             Colour = dao.BoardSetting.Colour
    //         },
    //         Team = new Team
    //         {
    //             Id = dao.Team.Id,
    //             Name = "Team 1"
    //         },
    //         ListBoards = dao.ListBoards.Select(BuildListBoard).ToArray()
    //     };
    // }
    //
    // private static ListBoard BuildListBoard(ListBoardDao dao) 
    //     => new()
    //     {
    //         Id = dao.Id,
    //         Name = dao.Name,
    //         Position = dao.Position,
    //         BoardId = dao.BoardId,
    //         Cards = dao.Cards.Select(BuildCard).ToArray()
    //     };
    //
    // private static Card BuildCard(CardDao dao)
    //     => new()
    //     {
    //         Id = dao.Id,
    //         Name = dao.Name,
    //         Description = dao.Description,
    //     };
    
    private async Task<BoardDao> Given_a_board()
    {
        var boardDao = new BoardDao
        {
            Name = "testBoard",
            BoardSetting = new BoardSettingDao
            {
                Colour = Colour.Gray
            },
            Team = new TeamDao
            {
                Name = "Team 1"
            }
        };

        DbContext.Boards.Add(boardDao);
        
        await DbContext.SaveChangesAsync();
        
        return boardDao;
    }
}
