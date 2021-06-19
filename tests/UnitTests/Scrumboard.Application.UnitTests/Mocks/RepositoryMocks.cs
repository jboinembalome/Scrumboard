using Ardalis.Specification;
using Moq;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Scrumboard.Application.UnitTests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IAsyncRepository<Board, int>> GetBoardRepository()
        {
            var adherent1Model = new Adherent 
            { 
                Id = 1, 
                IdentityGuid = "2cd08f87-33a6-4cbc-a0de-71d428986b85"
            };

            var team1 = new Team { Id = 1, Name = "Developer Team" };

            #region Fake data for the frontend board
            var labelsForFrontEndScrumboard = new Collection<Label>
            {
                new Label
                {
                    Id = 1,
                    Name = "Design",
                    Colour = Colour.White
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
                Adherent = adherent1Model,
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
                                Adherents = new Collection<Adherent> { adherent1Model },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = 1,
                                        Message = @"Jimmy Boinembalome moved Add Create login page on Design",
                                        Adherent = adherent1Model
                                    }
                                },
                                Attachments = new Collection<Attachment>
                                {
                                    new Attachment
                                    {
                                        Id = 1,
                                        Name = "Image.png",
                                        Url = "urlOfimage",
                                        AttachmentType = Domain.Enums.AttachmentType.Image
                                    },
                                     new Attachment
                                    {
                                        Id = 2,
                                        Name = "Image2.png",
                                        Url = "urlOfimage2",
                                        AttachmentType = Domain.Enums.AttachmentType.Image
                                    },
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
                                        Adherent = adherent1Model
                                    }
                                }
                            },
                            new Card
                            {
                                Id = 2,
                                Name = "Change background colors",
                                Description = null,
                                Suscribed = false,
                                DueDate = null,
                                Labels = new Collection<Label> { labelsForFrontEndScrumboard[0] },
                                Adherents = new Collection<Adherent> { },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = 2,
                                        Message = @"Jimmy Boinembalome added Change background colors on Design",
                                        Adherent = adherent1Model
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
                                Adherents = new Collection<Adherent> { },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = 3,
                                        Message = @"Jimmy Boinembalome added Fix splash screen bugs on Development",
                                        Adherent = adherent1Model
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
                                Adherents = new Collection<Adherent> { adherent1Model },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = 4,
                                        Message = @"Jimmy Boinembalome added Add a notification when a user adds a comment on Upcoming Features",
                                        Adherent = adherent1Model
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
                Labels = labelsForFrontEndScrumboard
            };
            #endregion

            #region Fake data for the backend board
            var labelsForBackEndScrumboard = new Collection<Label>
            {
                 new Label
                {
                    Id = 4,
                    Name = "Log",
                    Colour = Colour.White
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
                Adherent = adherent1Model,
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
                                Adherents = new Collection<Adherent> { adherent1Model },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = 5,
                                        Message = @"Jimmy Boinembalome added Write documentation for the naming convention on Backlog",
                                        Adherent = adherent1Model
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
                                Adherents = new Collection<Adherent> { },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = 6,
                                        Message = @"Jimmy Boinembalome added Add Serilog for logs on Backlog",
                                        Adherent = adherent1Model
                                    }
                                }
                            },
                        }
                    }
                },
                Labels = labelsForBackEndScrumboard
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

            mockBoardRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
              (int id, CancellationToken cancellationToken) =>
              {
                  var board = boards.FirstOrDefault(b => b.Id == id);
                  return board;
              });

            mockBoardRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ISpecification<Board>>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (ISpecification<Board> specification, CancellationToken cancellationToken) =>
                {
                    Board board = specification.Evaluate(boards).FirstOrDefault();
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

        public static Mock<IAsyncRepository<Adherent, int>> GetAdherentRepository()
        {
            var adherent1Model = new Adherent
            {
                Id = 1,
                IdentityGuid = "2cd08f87-33a6-4cbc-a0de-71d428986b85"
            };
        
            var adherents = new List<Adherent> { adherent1Model };

            var mockAdherentRepository = new Mock<IAsyncRepository<Adherent, int>>();
            mockAdherentRepository.Setup(repo => repo.ListAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(adherents);

            mockAdherentRepository.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<Adherent>>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                  (ISpecification<Adherent> specification, CancellationToken cancellationToken) =>
                  {
                      IReadOnlyList<Adherent> adherentList = specification.Evaluate(adherents).ToList().AsReadOnly();
                      return adherentList;
                  });

            mockAdherentRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
              (int id, CancellationToken cancellationToken) =>
              {
                  var adherent = adherents.FirstOrDefault(b => b.Id == id);
                  return adherent;
              });

            mockAdherentRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ISpecification<Adherent>>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (ISpecification<Adherent> specification, CancellationToken cancellationToken) =>
                {
                    Adherent adherent = specification.Evaluate(adherents).FirstOrDefault();
                    return adherent;
                });

            mockAdherentRepository.Setup(repo => repo.AddAsync(It.IsAny<Adherent>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (Adherent adherent, CancellationToken cancellationToken) =>
                {
                    adherent.Id = adherents.Count + 1;
                    adherents.Add(adherent);
                    return adherent;
                });

            mockAdherentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Adherent>(), It.IsAny<CancellationToken>())).Callback(
                (Adherent adherent, CancellationToken cancellationToken) =>
                {
                    var adherentToBeUpdated = adherents.FirstOrDefault(b => b.Id == adherent.Id);
                    adherentToBeUpdated = adherent;
                });

            mockAdherentRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Adherent>(), It.IsAny<CancellationToken>())).Callback(
               (Adherent adherent, CancellationToken cancellationToken) =>
               {
                   adherents.Remove(adherent);
               });

            return mockAdherentRepository;
        }

    }
}
