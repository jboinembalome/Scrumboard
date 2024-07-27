using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Persistence.Teams;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards;

// [Collection("Database collection")]
// public class BoardRepositoryTests(
//     DatabaseFixture database) : IAsyncLifetime
// {
//     public async Task DisposeAsync() =>  await database.ResetState();
//
//     public Task InitializeAsync() => Task.CompletedTask;
//     
//     [Fact]
//     public async Task AddAsync_ValidBoard_BoardAdded()
//     {           
//         // Arrange
//         var testBoardName = "testBoard";
//         var board = new Board
//         {
//             Name = testBoardName,
//             BoardSetting = new BoardSetting
//             {
//                 Colour = Colour.Gray
//             },
//             Team = new Team
//             {
//                 Name = "Team 1"
//             }
//         };
//         var boardRepository =  database.GetRepository<Board, int>();
//
//         // Act
//         var newboard = await boardRepository.AddAsync(board);
//         var boardAdded = (await database.DbContext.Boards.ToListAsync())[0];
//
//         // Assert
//         boardAdded.Name.Should().Be(newboard.Name);
//         boardAdded.Id.Should().BeGreaterThan(0);
//     }
//     
//     [Fact]
//     public async Task DeleteAsync_ExistingBoard_BoardDeleted()
//     {           
//         // Arrange
//         var testBoardName = "testBoard";
//         var boardDao = new BoardDao
//         {
//             Name = testBoardName,
//             BoardSetting = new BoardSettingDao
//             {
//                 Colour = Colour.Gray
//             },
//             Team = new TeamDao
//             {
//                 Name = "Team 1"
//             }
//         };
//         var boardRepository =  database.GetRepository<Board, int>();
//
//         database.DbContext.Boards.Add(boardDao);
//         await database.DbContext.SaveChangesAsync();
//
//         var board = BuildBoard(boardDao);
//         
//         // Act
//         await boardRepository.DeleteAsync(board);
//         var boardDeleted = await database.DbContext.Boards.FirstOrDefaultAsync(b => b.Name == testBoardName);
//
//         // Assert
//         boardDeleted.Should().BeNull();
//     }
//     
//     [Fact]
//     public async Task GetByIdAsync_ExistingId_BoardRetrieved()
//     {           
//         // Arrange
//         var testBoardName = "testBoard";
//         
//         var board = new BoardDao
//         {
//             Name = testBoardName,
//             BoardSetting = new BoardSettingDao
//             {
//                 Colour = Colour.Gray
//             },
//             Team = new TeamDao
//             {
//                 Name = "Team 1"
//             }
//         };
//         var boardRepository =  database.GetRepository<Board, int>();
//
//         database.DbContext.Boards.Add(board);
//         await database.DbContext.SaveChangesAsync();
//
//         int boardId = board.Id;
//
//         // Act
//         var boardRetrived = await boardRepository.TryGetByIdAsync(boardId);
//
//         // Assert
//         boardRetrived!.Name.Should().Be(board.Name);
//         boardRetrived.Id.Should().BeGreaterThan(0);
//     }
//     
//     [Fact]
//     public async Task UpdateAsync_ExistingBoard_BoardUpdated()
//     {           
//         // Arrange
//         var testBoardName = "testBoard";
//         var boardDao = new BoardDao
//         {
//             Name = testBoardName,
//             BoardSetting = new BoardSettingDao
//             {
//                 Colour = Colour.Gray
//             },
//             Team = new TeamDao
//             {
//                 Name = "Team 1"
//             }
//         };
//         var boardRepository =  database.GetRepository<Board, int>();
//
//         database.DbContext.Boards.Add(boardDao);
//         await database.DbContext.SaveChangesAsync();
//
//         boardDao.Name = "Updated Name";
//
//         var board = BuildBoard(boardDao);
//         
//         // Act
//         await boardRepository.UpdateAsync(board);
//         var boardUpdated = await database.DbContext.Boards.FirstOrDefaultAsync(b => b.Name == boardDao.Name);
//
//         // Assert
//         boardUpdated.Should().NotBeNull();
//         boardUpdated!.Id.Should().Be(boardDao.Id);
//         boardUpdated.Name.Should().Be(boardDao.Name);        
//     }
//
//     private static Board BuildBoard(BoardDao dao)
//     {
//         return new Board
//         {
//             Id = dao.Id,
//             Name = dao.Name,
//             Uri = dao.Uri,
//             IsPinned = dao.IsPinned,
//             BoardSetting = new BoardSetting
//             {
//                 Id = dao.BoardSetting.Id,
//                 Colour = dao.BoardSetting.Colour
//             },
//             Team = new Team
//             {
//                 Id = dao.Team.Id,
//                 Name = "Team 1"
//             },
//             ListBoards = dao.ListBoards.Select(BuildListBoard).ToArray()
//         };
//     }
//
//     private static ListBoard BuildListBoard(ListBoardDao dao) 
//         => new()
//         {
//             Id = dao.Id,
//             Name = dao.Name,
//             Position = dao.Position,
//             BoardId = dao.BoardId,
//             Cards = dao.Cards.Select(BuildCard).ToArray()
//         };
//
//     private static Card BuildCard(CardDao dao)
//         => new()
//         {
//             Id = dao.Id,
//             Name = dao.Name,
//             Description = dao.Description,
//         };
// }
