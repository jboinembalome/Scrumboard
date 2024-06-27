using Ardalis.Specification;
using Moq;
using System.Collections.ObjectModel;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.UnitTests.Mocks;

public class RepositoryMocks
{
    // TODO: Move mock
    public static Mock<IAsyncRepository<Board, int>> GetBoardRepository()
    {
        var adherent1Model = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85");

        var team1 = new Team 
        { 
            Id = 1, 
            Name = "Developer Team",
            Adherents = new Collection<Guid>{ adherent1Model }
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
                            Labels = new Collection<Label> { labelsForFrontEndScrumboard[0], labelsForFrontEndScrumboard[1] },
                            Assignees = new Collection<Guid>
                            {
                                adherent1Model
                            },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Id = 1,
                                    NewValue = @"Jimmy Boinembalome moved Add Create login page on Design",
                                    CreatedBy = adherent1Model
                                }
                            },
                            Checklists = new Collection<Checklist>
                            {
                                new Checklist
                                {
                                    Id = 1,
                                    Name = "Checklist",
                                    ChecklistItems = new Collection<ChecklistItem>
                                    {
                                        new ChecklistItem
                                        {
                                            Id = 1,
                                            Name = "Create template for the login page",
                                            IsChecked = true,
                                        },
                                        new ChecklistItem
                                        {
                                            Id = 2,
                                            Name = "Validate template for the login page",
                                            IsChecked = false,
                                        }
                                    }
                                }
                            },
                            Comments = new Collection<Comment>
                            {
                                new Comment
                                {
                                    Id = 1,
                                    Message = "The template for the login page is available on the cloud.",
                                    CreatedBy = adherent1Model
                                }
                            }
                        },
                        new Card
                        {
                            Id = 2,
                            Name = "Change background colors",
                            Description = null!,
                            Suscribed = false,
                            DueDate = null,
                            Labels = new Collection<Label> { labelsForFrontEndScrumboard[0] },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Id = 2,
                                    NewValue = @"Jimmy Boinembalome added Change background colors on Design",
                                    CreatedBy = adherent1Model
                                }
                            }
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
                            Labels = new Collection<Label> { labelsForFrontEndScrumboard[1] },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Id = 3,
                                    NewValue = @"Jimmy Boinembalome added Fix splash screen bugs on Development",
                                    CreatedBy = adherent1Model
                                }
                            }
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
                            Labels = new Collection<Label> { labelsForFrontEndScrumboard[2] },
                            Assignees = new Collection<Guid>
                            {
                                adherent1Model
                            },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Id = 4,
                                    NewValue = @"Jimmy Boinembalome added Add a notification when a user adds a comment on Upcoming Features",
                                    CreatedBy = adherent1Model
                                }
                            }
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
                            Labels = new Collection<Label> { labelsForBackEndScrumboard[1] },
                            Assignees = new Collection<Guid>
                            {
                                adherent1Model
                            },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Id = 5,
                                    NewValue = @"Jimmy Boinembalome added Write documentation for the naming convention on Backlog",
                                    CreatedBy = adherent1Model
                                }
                            }
                        },
                        new Card
                        {
                            Id = 6,
                            Name = "Add Serilog for logs",
                            Description = "",
                            Suscribed = false,
                            DueDate = null,
                            Labels = new Collection<Label> { labelsForBackEndScrumboard[0] },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Id = 6,
                                    NewValue = @"Jimmy Boinembalome added Add Serilog for logs on Backlog",
                                    CreatedBy = adherent1Model
                                }
                            }
                        },
                    }
                }
            },
        };
        #endregion

        var boards = new List<Board> { boardFrontEndScrumboard, boardScrumboardBackEnd };

        var mockBoardRepository = new Mock<IAsyncRepository<Board, int>>();
        mockBoardRepository.Setup(repo => repo.ListAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(boards);

        mockBoardRepository.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<Board>>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            (ISpecification<Board> specification, CancellationToken cancellationToken) =>
            {
                IReadOnlyList<Board> boardList = specification.Evaluate(boards).ToList().AsReadOnly();
                return boardList;
            });

        mockBoardRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(
            (int id, CancellationToken cancellationToken) =>
            {
                var board = boards.FirstOrDefault(b => b.Id == id);
                return board;
            });

        mockBoardRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ISpecification<Board>>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            (ISpecification<Board?> specification, CancellationToken cancellationToken) =>
            {
                Board? board = specification.Evaluate(boards).FirstOrDefault();
                return board;
            });

        mockBoardRepository.Setup(repo => repo.AddAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            (Board board, CancellationToken cancellationToken) =>
            {
                board.Id = boards.Count + 1;
                boards.Add(board);
                return board;
            });

        mockBoardRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>())).Callback(
            (Board board, CancellationToken cancellationToken) =>
            {
                var boardToBeUpdated = boards.FirstOrDefault(b => b.Id == board.Id);
                boardToBeUpdated = board;
            });

        mockBoardRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>())).Callback(
            (Board board, CancellationToken cancellationToken) =>
            {
                boards.Remove(board);
            });

        return mockBoardRepository;
    }
}
