using Ardalis.Specification;
using Moq;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Scrumboard.Application.UnitTests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IAsyncRepository<Board, Guid>> GetBoardRepository()
        {
            var adherent1Guid = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85");
            var team1 = new Team { Id = Guid.Parse("6161ac4d-158e-40e5-873f-f335fcd682f5"), Name = "Developer Team" };

            #region Fake data for the frontend board
            var labelsForFrontEndScrumboard = new Collection<Label>
            {
                new Label
                {
                    Id = Guid.Parse("059eecf9-074e-4a9a-aaa2-4b46d7fa2bd5"),
                    Name = "Design",
                    CustomColor = Domain.Enums.CustomColor.White
                },
                new Label
                {
                    Id = Guid.Parse("3bbb0e95-2cf0-4f78-b4a8-f6c8f3cc0fab"),
                    Name = "App",
                    CustomColor = Domain.Enums.CustomColor.Orange
                },
                new Label
                {
                    Id = Guid.Parse("fd9b02cc-6086-4928-81a2-d07c3552d8ca"),
                    Name = "Feature",
                    CustomColor = Domain.Enums.CustomColor.Black
                }
            };
            var boardFrontEndScrumboard = new Board
            {
                Id = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"),
                Name = "Scrumboard FrontEnd",
                Uri = "scrumboard-frontend",
                UserId = adherent1Guid,
                Team = team1,
                ListBoards = new Collection<ListBoard>
                {
                    new ListBoard
                    {
                        Id = Guid.Parse("eb801edc-e076-4dcb-ad8c-270605644fbf"),
                        Name = "Design",
                        Cards = new Collection<Card>
                        {
                            new Card
                            {
                                Id = Guid.Parse("42cd1f61-da92-4aaa-b6ee-a586b7426c1a"),
                                Name = "Create login page",
                                Description = "Create login page with social network authenfication.",
                                Suscribed = false,
                                DueDate = null,
                                Labels = new Collection<Label> { labelsForFrontEndScrumboard[0], labelsForFrontEndScrumboard[1] },
                                UserIds = new Collection<Guid> { adherent1Guid },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = Guid.Parse("a90e5b63-0f3f-4a04-9b18-6916b4338bfc"),
                                        Message = @"Jimmy Boinembalome moved Add Create login page on Design",
                                        UserId = adherent1Guid
                                    }
                                },
                                Attachments = new Collection<Attachment>
                                {
                                    new Attachment
                                    {
                                        Id = Guid.Parse("c61ea92e-c704-4d98-ba40-c42b10ccc471"),
                                        Name = "Image.png",
                                        Url = "urlOfimage",
                                        AttachmentType = Domain.Enums.AttachmentType.Image
                                    },
                                     new Attachment
                                    {
                                        Id = Guid.Parse("f36cd7f8-71b7-475c-9a86-08ab5f87c40d"),
                                        Name = "Image2.png",
                                        Url = "urlOfimage2",
                                        AttachmentType = Domain.Enums.AttachmentType.Image
                                    },
                                },
                                Checklists = new Collection<Checklist>
                                {
                                    new Checklist
                                    {
                                        Id = Guid.Parse("fd06ca29-8f39-4268-9818-155152230c05"),
                                        Name = "Checklist",
                                        ChecklistItems = new Collection<ChecklistItem>
                                        {
                                            new ChecklistItem
                                            {
                                                Id = Guid.Parse("dd2f3d96-428c-4605-8e2b-f65a7007a151"),
                                                Name = "Create template for the login page",
                                                IsChecked = true,
                                            },
                                            new ChecklistItem
                                            {
                                                Id = Guid.Parse("671c6398-a74c-440a-a6bd-5a2240714189"),
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
                                        Id = Guid.Parse("f256adda-8712-4aa6-939e-fb152df56361"),
                                        Message = "The template for the login page is available on the cloud.",
                                        UserId = adherent1Guid
                                    }
                                }
                            },
                            new Card
                            {
                                Id = Guid.Parse("a527a1ff-21af-4719-8030-19ac27f68897"),
                                Name = "Change background colors",
                                Description = null,
                                Suscribed = false,
                                DueDate = null,
                                Labels = new Collection<Label> { labelsForFrontEndScrumboard[0] },
                                UserIds = new Collection<Guid> { },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = Guid.Parse("a90e5b63-0f3f-4a04-9b18-6916b4338bfc"),
                                        Message = @"Jimmy Boinembalome added Change background colors on Design",
                                        UserId = adherent1Guid
                                    }
                                }
                            }
                        }
                    },
                    new ListBoard
                    {
                        Id = Guid.Parse("2d60c9f9-7144-4687-a77a-4d273f1356d3"),
                        Name = "Development",
                        Cards = new Collection<Card>
                        {
                            new Card
                            {
                                Id = Guid.Parse("0ffe6c1b-adda-4900-8ac1-654b275b8a55"),
                                Name = "Fix splash screen bugs",
                                Description = "",
                                Suscribed = true,
                                DueDate = new DateTime(2021, 5, 15),
                                Labels = new Collection<Label> { labelsForFrontEndScrumboard[1] },
                                UserIds = new Collection<Guid> { },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = Guid.Parse("323f21fd-d8d0-42fb-8c1d-dc745fa5bb8a"),
                                        Message = @"Jimmy Boinembalome added Fix splash screen bugs on Development",
                                        UserId = adherent1Guid
                                    }
                                }
                            },
                        }
                    },
                    new ListBoard
                    {
                        Id = Guid.Parse("13cbad90-42a5-4ac9-bbfe-da808ff811a9"),
                        Name = "Upcoming Features",
                        Cards = new Collection<Card>
                        {
                            new Card
                            {
                                Id = Guid.Parse("ac7dd674-fd33-4404-86a1-6650fc401398"),
                                Name = "Add a notification when a user adds a comment",
                                Description = "",
                                Suscribed = false,
                                DueDate = null,
                                Labels = new Collection<Label> { labelsForFrontEndScrumboard[2] },
                                UserIds = new Collection<Guid> { adherent1Guid },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = Guid.Parse("631a9d6c-a512-4ad8-a374-62b74732a283"),
                                        Message = @"Jimmy Boinembalome added Add a notification when a user adds a comment on Upcoming Features",
                                        UserId = adherent1Guid
                                    }
                                }
                            },
                        }
                    },
                    new ListBoard
                    {
                        Id = Guid.Parse("08a13f33-801a-4ebe-94d9-d4b0a0944c45"),
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
                    Id = Guid.Parse("0a6d0625-2019-483d-bea1-c0776253a1a5"),
                    Name = "Log",
                    CustomColor = Domain.Enums.CustomColor.White
                },
                new Label
                {
                    Id = Guid.Parse("98cdcc99-510d-429e-9e9f-2ee33c5d6fa8"),
                    Name = "Documentation",
                    CustomColor = Domain.Enums.CustomColor.Orange
                },
                new Label
                {
                    Id = Guid.Parse("492dcef8-ea07-4656-9941-7c4c6f4eb00b"),
                    Name = "Persitence",
                    CustomColor = Domain.Enums.CustomColor.Black
                }
            };
            var boardScrumboardBackEnd = new Board
            {
                Id = Guid.Parse("6313179F-7837-473A-A4D5-A5571B43E6A6"),
                Name = "Scrumboard BackEnd",
                Uri = "scrumboard-backend",
                UserId = adherent1Guid,
                Team = team1,
                ListBoards = new Collection<ListBoard>
                {
                    new ListBoard
                    {
                        Id = Guid.Parse("283c03ce-64f6-469b-8a88-eb6f8211a020"),
                        Name = "Backlog",
                        Cards = new Collection<Card>
                        {
                            new Card
                            {
                                Id = Guid.Parse("00d4de56-1d3e-40e2-9ef2-21f4ff74d753"),
                                Name = "Write documentation for the naming convention",
                                Description = "",
                                Suscribed = false,
                                DueDate = null,
                                Labels = new Collection<Label> { labelsForBackEndScrumboard[1] },
                                UserIds = new Collection<Guid> { adherent1Guid },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = Guid.Parse("2d4bb05c-ac70-4233-8bce-973b99ed053f"),
                                        Message = @"Jimmy Boinembalome added Write documentation for the naming convention on Backlog",
                                        UserId = adherent1Guid
                                    }
                                }
                            },
                            new Card
                            {
                                Id = Guid.Parse("9dbb1206-c374-43d9-9a80-4da984e03b18"),
                                Name = "Add Serilog for logs",
                                Description = "",
                                Suscribed = false,
                                DueDate = null,
                                Labels = new Collection<Label> { labelsForBackEndScrumboard[0] },
                                UserIds = new Collection<Guid> { },
                                Activities =  new Collection<Activity>
                                {
                                    new Activity
                                    {
                                        Id = Guid.Parse("1474b655-22d6-4eaa-8a19-e29b28551475"),
                                        Message = @"Jimmy Boinembalome added Add Serilog for logs on Backlog",
                                        UserId = adherent1Guid
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

            var mockBoardRepository = new Mock<IAsyncRepository<Board, Guid>>();
            mockBoardRepository.Setup(repo => repo.ListAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(boards);

            mockBoardRepository.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<Board>>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                  (ISpecification<Board> specification, CancellationToken cancellationToken) =>
                  {
                      IReadOnlyList<Board> boardList = specification.Evaluate(boards).ToList().AsReadOnly();
                      return boardList;
                  });

            mockBoardRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(
              (Guid id, CancellationToken cancellationToken) =>
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
}
