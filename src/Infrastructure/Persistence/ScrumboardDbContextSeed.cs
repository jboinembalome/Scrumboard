using Microsoft.AspNetCore.Identity;
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
            var adherent1 = new Adherent
            {
                IdentityGuid = ADHERENT_USER_ID
            };

            if (!context.Adherents.Any())
            {
                context.Adherents.Add(adherent1);
                await context.SaveChangesAsync();
            }
                
            var team1 = new Team { Name = "Developer Team" };

            if (!context.Teams.Any())
            {
                context.Teams.Add(team1);
                await context.SaveChangesAsync();
            }
                

            var labelsForFrontEndScrumboard = new Collection<Label>
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
                }
            };

            if (!context.Labels.Any())
            {
                context.Labels.Add(labelsForFrontEndScrumboard[0]);
                context.Labels.Add(labelsForFrontEndScrumboard[1]);
                context.Labels.Add(labelsForFrontEndScrumboard[2]);
                await context.SaveChangesAsync();
            }


            // Seed, if necessary
            if (!context.Boards.Any())
            {
                context.Boards.Add(new Board
                {
                    Name = "Scrumboard FrontEnd",
                    Uri = "scrumboard-frontend",
                    Adherent = adherent1,
                    Team = team1,
                    ListBoards = new Collection<ListBoard>
                    {
                        new ListBoard
                        {
                            Name = "Design",
                            Cards = new Collection<Card>
                            {
                                new Card
                                {
                                    Name = "Create login page",
                                    Description = "Create login page with social network authenfication.",
                                    Suscribed = false,
                                    DueDate = null,
                                    Labels = new Collection<Label> { labelsForFrontEndScrumboard[0], labelsForFrontEndScrumboard[1] },
                                    Adherents = new Collection<Adherent> { adherent1 },
                                    Activities =  new Collection<Activity>
                                    {
                                        new Activity
                                        {
                                            Message = @"Jimmy Boinembalome moved Add Create login page on Design",
                                            Adherent = adherent1
                                        }
                                    },
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
                                            Adherent = adherent1
                                        }
                                    }
                                },
                                new Card
                                {
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
                                            Message = @"Jimmy Boinembalome added Change background colors on Design",
                                            Adherent = adherent1
                                        }
                                    }
                                }
                            }
                        },
                        new ListBoard
                        {
                            Name = "Development",
                            Cards = new Collection<Card>
                            {
                                new Card
                                {
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
                                            Message = @"Jimmy Boinembalome added Fix splash screen bugs on Development",
                                            Adherent = adherent1
                                        }
                                    }
                                },
                            }
                        },
                        new ListBoard
                        {
                            Name = "Upcoming Features",
                            Cards = new Collection<Card>
                            {
                                new Card
                                {
                                    Name = "Add a notification when a user adds a comment",
                                    Description = "",
                                    Suscribed = false,
                                    DueDate = null,
                                    Labels = new Collection<Label> { labelsForFrontEndScrumboard[2] },
                                    Adherents = new Collection<Adherent> { adherent1 },
                                    Activities =  new Collection<Activity>
                                    {
                                        new Activity
                                        {
                                            Message = @"Jimmy Boinembalome added Add a notification when a user adds a comment on Upcoming Features",
                                            Adherent = adherent1
                                        }
                                    }
                                },
                            }
                        },
                        new ListBoard
                        {
                            Name = "Known Bugs",
                            Cards = new Collection<Card>{ }
                        }
                    },
                    Labels = labelsForFrontEndScrumboard
                });

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
