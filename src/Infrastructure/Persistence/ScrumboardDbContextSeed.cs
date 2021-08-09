using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.ValueObjects;
using Scrumboard.Infrastructure.Identity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Scrumboard.Infrastructure.Persistence
{
    public static class ScrumboardDbContextSeed
    {
        private const string ADMIN_USER_ID = "31a7ffcf-d099-4637-bd58-2a87641d1aaf";
        private const string ADHERENT_USER_ID = "533f27ad-d3e8-4fe7-9259-ee4ef713dbea";

        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await CreateUser(userManager, roleManager, "Administrator", ADMIN_USER_ID, "administrator@localhost", "administrator@localhost", "Administrator1!");
            await CreateUser(userManager, roleManager, "Adherent", ADHERENT_USER_ID, "adherent@localhost", "adherent@localhost", "Adherent1!");
        }

        public static async Task SeedSampleDataAsync(ScrumboardDbContext context)
        {
            var adherent = new Adherent
            {
                IdentityGuid = ADHERENT_USER_ID
            };

            var team = new Team { Name = "Developer Team", Adherents = new Collection<Adherent>() { adherent} };

            var labels = new Collection<Label>
            {
                new Label
                {
                    Name = "Design",
                    Colour = Colour.White
                },
                new Label
                {
                    Name = "App",
                    Colour = Colour.Orange
                },
                new Label
                {
                    Name = "Feature",
                    Colour = Colour.Red
                },
                new Label
                {
                    Name = "Log",
                    Colour = Colour.White
                },
                new Label
                {
                    Name = "Documentation",
                    Colour = Colour.Orange
                },
                new Label
                {
                    Name = "Persitence",
                    Colour = Colour.Red
                }
            };

            var boardSettings = new Collection<BoardSetting>
            {
                new BoardSetting
                {
                    Colour = Colour.Red,                 
                },
                new BoardSetting
                {
                    Colour = Colour.Yellow,
                }, 
                new BoardSetting
                {
                    Colour = Colour.Blue,
                },
            };


            var activities = new Collection<Activity>
            {
                new Activity
                 {
                     Message = @"Jimmy Boinembalome moved Add Create login page on Design",
                     Adherent = adherent
                 },
                new Activity
                {
                    Message = @"Jimmy Boinembalome added Change background colors on Design",
                    Adherent = adherent
                },
                new Activity
                {
                    Message = @"Jimmy Boinembalome added Fix splash screen bugs on Development",
                    Adherent = adherent
                },
                new Activity
                {
                    Message = @"Jimmy Boinembalome added Add a notification when a user adds a comment on Upcoming Features",
                    Adherent = adherent
                }
            };

            var cards = new Collection<Card>
            {
                new Card
                {
                    Name = "Create login page",
                    Description = "Create login page with social network authenfication.",
                    Suscribed = false,
                    DueDate = null,
                    Labels = new Collection<Label> { labels[0], labels[1] },
                    Adherents = new Collection<Adherent> { adherent },
                    Activities =  new Collection<Activity> { activities[0] },
                    Attachments = new Collection<Attachment>
                    {
                        new Attachment
                        {
                            Name = "Image.png",
                            Url = "urlOfimage",
                            AttachmentType = Domain.Enums.AttachmentType.Image
                        },
                            new Attachment
                        {
                            Name = "Image2.png",
                            Url = "urlOfimage2",
                            AttachmentType = Domain.Enums.AttachmentType.Image
                        },
                    },
                    Checklists = new Collection<Checklist>
                    {
                        new Checklist
                        {
                            Name = "Checklist",
                            ChecklistItems = new Collection<ChecklistItem>
                            {
                                new ChecklistItem
                                {
                                    Name = "Create template for the login page",
                                    IsChecked = true,
                                },
                                new ChecklistItem
                                {
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
                            Message = "The template for the login page is available on the cloud.",
                            Adherent = adherent
                        }
                    }
                },
                new Card
                {
                    Name = "Change background colors",
                    Description = null,
                    Suscribed = false,
                    DueDate = null,
                    Labels = new Collection<Label> { labels[0] },
                    Activities =  new Collection<Activity> { activities[1] }
                },
                new Card
                {
                    Name = "Fix splash screen bugs",
                    Description = "",
                    Suscribed = true,
                    DueDate = new DateTime(2021, 5, 15),
                    Labels = new Collection<Label> { labels[1] },
                    Activities =  new Collection<Activity> { activities[2] }
                },
                new Card
                {
                    Name = "Add a notification when a user adds a comment",
                    Description = "",
                    Suscribed = false,
                    DueDate = null,
                    Labels = new Collection<Label> { labels[2] },
                    Adherents = new Collection<Adherent> { adherent },
                    Activities =  new Collection<Activity> { activities[3] }
                },
            };

            var listboards = new Collection<ListBoard>
            {
                new ListBoard
                {
                    Name = "Design",
                    Cards = new Collection<Card>{ cards[0], cards[1] }
                },
                new ListBoard
                {
                    Name = "Development",
                    Cards = new Collection<Card> { cards[2] }
                },
                new ListBoard
                {
                    Name = "Upcoming Features",
                    Cards = new Collection<Card> { cards[3] }
                },
                new ListBoard
                {
                    Name = "Known Bugs",
                },
                new ListBoard
                {
                    Name = "Backlog",
                    Cards = new Collection<Card>
                    {
                        new Card
                        {
                            Name = "Write documentation for the naming convention",
                            Description = "",
                            Suscribed = false,
                            DueDate = null,
                            Labels = new Collection<Label> { labels[4] },
                            Adherents = new Collection<Adherent> { adherent },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Message = @"Jimmy Boinembalome added Write documentation for the naming convention on Backlog",
                                    Adherent = adherent
                                }
                            }
                        },
                        new Card
                        {
                            Name = "Add Serilog for logs",
                            Description = "",
                            Suscribed = false,
                            DueDate = null,
                            Labels = new Collection<Label> { labels[3] },
                            Adherents = new Collection<Adherent> { },
                            Activities =  new Collection<Activity>
                            {
                                new Activity
                                {
                                    Message = @"Jimmy Boinembalome added Add Serilog for logs on Backlog",
                                    Adherent = adherent
                                }
                            }
                        },
                    }
                }
            };

            var boards = new Collection<Board>
            {
                new Board
                {
                    Name = "Scrumboard Frontend",
                    Uri = "scrumboard-frontend",
                    IsPinned = false,
                    Adherent = adherent,
                    Team = team,
                    ListBoards = new Collection<ListBoard> { listboards[0], listboards[1], listboards[2], listboards[3] },
                    Labels = labels,
                    BoardSetting = boardSettings[0]
                },
                new Board
                {
                    Name = "Scrumboard Backend",
                    Uri = "scrumboard-backend",
                    IsPinned = true,
                    Adherent = adherent,
                    Team = team,
                    ListBoards = new Collection<ListBoard> { listboards[4] },
                    BoardSetting = boardSettings[1]

                },
                new Board
                {
                    Name = "Scrumboard Test",
                    Uri = "scrumboard-test",
                    Adherent = adherent,
                    BoardSetting = boardSettings[2]
                }
            };

            // Seed, if necessary
            if (!await context.Boards.AnyAsync())
            {
                await context.Boards.AddRangeAsync(boards);

                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, string role, string userId, string userName, string userMail, string userPassword)
        {
            var identityRole = new IdentityRole(role);

            if (roleManager.Roles.All(r => r.Name != identityRole.Name))
                await roleManager.CreateAsync(identityRole);

            var administrator = new ApplicationUser { Id = userId, UserName = userName, Email = userMail };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                var test = await userManager.CreateAsync(administrator, userPassword);
                await userManager.AddToRolesAsync(administrator, new[] { identityRole.Name });
            }
        }
    }
}
