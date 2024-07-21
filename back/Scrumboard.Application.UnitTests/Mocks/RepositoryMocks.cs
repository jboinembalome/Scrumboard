using Moq;
using System.Collections.ObjectModel;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.UnitTests.Mocks;

public class RepositoryMocks
{
    // TODO: Move mock
    public static Mock<IBoardsRepository> GetBoardRepository()
    {
        var user1Model = "2cd08f87-33a6-4cbc-a0de-71d428986b85";

        var team1 = new Team 
        { 
            Id = 1, 
            Name = "Developer Team",
            MemberIds = [user1Model]
        };

        #region Fake data for the frontend board
        var labelsForFrontEndScrumboard = new Collection<Label>
        {
            new Label
            {
                Id = 1,
                Name = "Design",
                Colour = Colour.Blue
            },
            new Label
            {
                Id = 2,
                Name = "App",
                Colour = Colour.Orange
            },
            new Label
            {
                Id = 3,
                Name = "Feature",
                Colour = Colour.Red
            }
        };
        var boardFrontEndScrumboard = new Board
        {
            Id = 1,
            Name = "Scrumboard FrontEnd",
            Uri = "scrumboard-frontend",
            Team = team1,
            ListBoards = new Collection<ListBoard>
            {
                new ListBoard
                {
                    Id = 1,
                    Name = "Design",
                    Cards = new Collection<Card>
                    {
                        new Card
                        {
                            Id = 1,
                            Name = "Create login page",
                            Description = "Create login page with social network authenfication.",
                            Suscribed = false,
                            DueDate = null,
                            LabelIds = new Collection<int> { labelsForFrontEndScrumboard[0].Id, labelsForFrontEndScrumboard[1].Id },
                            AssigneeIds = [user1Model]
                        },
                        new Card
                        {
                            Id = 2,
                            Name = "Change background colors",
                            Description = null!,
                            Suscribed = false,
                            DueDate = null,
                            LabelIds = new Collection<int> { labelsForFrontEndScrumboard[0].Id }
                        }
                    }
                },
                new ListBoard
                {
                    Id = 2,
                    Name = "Development",
                    Cards = new Collection<Card>
                    {
                        new Card
                        {
                            Id = 3,
                            Name = "Fix splash screen bugs",
                            Description = "",
                            Suscribed = true,
                            DueDate = new DateTime(2021, 5, 15),
                            LabelIds = new Collection<int> { labelsForFrontEndScrumboard[1].Id }
                        },
                    }
                },
                new ListBoard
                {
                    Id = 3,
                    Name = "Upcoming Features",
                    Cards = new Collection<Card>
                    {
                        new Card
                        {
                            Id = 4,
                            Name = "Add a notification when a user adds a comment",
                            Description = "",
                            Suscribed = false,
                            DueDate = null,
                            LabelIds = new Collection<int> { labelsForFrontEndScrumboard[2].Id },
                            AssigneeIds = [user1Model]
                        },
                    }
                },
                new ListBoard
                {
                    Id = 4,
                    Name = "Known Bugs",
                    Cards = new Collection<Card>{ }
                }
            },
        };
        #endregion

        #region Fake data for the backend board
        var labelsForBackEndScrumboard = new Collection<Label>
        {
            new Label
            {
                Id = 4,
                Name = "Log",
                Colour = Colour.Blue
            },
            new Label
            {
                Id = 5,
                Name = "Documentation",
                Colour = Colour.Orange
            },
            new Label
            {
                Id = 6,
                Name = "Persitence",
                Colour = Colour.Red
            }
        };
        var boardScrumboardBackEnd = new Board
        {
            Id = 2,
            Name = "Scrumboard BackEnd",
            Uri = "scrumboard-backend",
            Team = team1,
            ListBoards = new Collection<ListBoard>
            {
                new ListBoard
                {
                    Id = 5,
                    Name = "Backlog",
                    Cards = new Collection<Card>
                    {
                        new Card
                        {
                            Id = 5,
                            Name = "Write documentation for the naming convention",
                            Description = "",
                            Suscribed = false,
                            DueDate = null,
                            LabelIds = new Collection<int> { labelsForBackEndScrumboard[1].Id },
                            AssigneeIds = [user1Model]
                        },
                        new Card
                        {
                            Id = 6,
                            Name = "Add Serilog for logs",
                            Description = "",
                            Suscribed = false,
                            DueDate = null,
                            LabelIds = new Collection<int> { labelsForBackEndScrumboard[0].Id }
                        },
                    }
                }
            },
        };
        #endregion

        var boards = new List<Board> { boardFrontEndScrumboard, boardScrumboardBackEnd };

        var mockBoardRepository = new Mock<IBoardsRepository>();

        mockBoardRepository.Setup(repo => repo.TryGetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(
            (int id, CancellationToken cancellationToken) =>
            {
                var board = boards.FirstOrDefault(b => b.Id == id);
                return board;
            });

        mockBoardRepository.Setup(repo => repo.AddAsync(It.IsAny<BoardCreation>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            (Board board, CancellationToken cancellationToken) =>
            {
                board.Id = boards.Count + 1;
                boards.Add(board);
                return board;
            });

        mockBoardRepository.Setup(repo => repo.UpdateAsync(It.IsAny<BoardEdition>(), It.IsAny<CancellationToken>())).Callback(
            (Board board, CancellationToken cancellationToken) =>
            {
                var boardToBeUpdated = boards.First(b => b.Id == board.Id);
                boardToBeUpdated = board;
                
                return boardToBeUpdated; 
            });

        mockBoardRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).Callback(
            (int id, CancellationToken cancellationToken) =>
            {
                var boardToBeRemoved = boards.First(b => b.Id == id);
                boards.Remove(boardToBeRemoved);
            });

        return mockBoardRepository;
    }
}
