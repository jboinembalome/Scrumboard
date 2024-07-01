using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Persistence.Adherents;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards.ListBoards;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.Cards.Checklists;
using Scrumboard.Infrastructure.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence;

public static class ScrumboardDbContextSeed
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ScrumboardDbContextInitializer>();

        await initializer.InitialiseAsync();

        await initializer.SeedAsync();
    }
}

public class ScrumboardDbContextInitializer(
    ILogger<ScrumboardDbContextInitializer> logger,
    ScrumboardDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    private const string AdherentUserId = "533f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private const string AdherentUserId2 = "633f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private const string AdherentUserId3 = "635f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private const string AdherentUserId4 = "637f27ad-d3e8-4fe7-9259-ee4ef713dbea";

    public async Task InitialiseAsync()
    {
        if (!context.Database.IsSqlServer())
        {
            return;
        }

        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedDefaultUserAsync(userManager, roleManager);
        await SeedSampleDataAsync(context);
    }

    private static async Task SeedDefaultUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Adherent" };
        
        var adherent = new ApplicationUser
        {
            Id = AdherentUserId,
            FirstName = "Jimmy",
            LastName = "Boinembalome",
            UserName = "adherent@localhost",
            Email = "adherent@localhost",
            Job = "Software Engineer",
            Avatar = []
        };
        var adherent2 = new ApplicationUser
        {
            Id = AdherentUserId2,
            FirstName = "Guyliane",
            LastName = "De Jesus Pimenta",
            UserName = "adherent2@localhost",
            Email = "adherent2@localhost",
            Job = "Software Engineer",
            Avatar = []
        };
        var adherent3 = new ApplicationUser
        {
            Id = AdherentUserId3,
            FirstName = "Corentin",
            LastName = "Hugot",
            UserName = "adherent3@localhost",
            Email = "adherent3@localhost",
            Job = "Systems and Networks Engineer",
            Avatar = []
        };
        var adherent4 = new ApplicationUser
        {
            Id = AdherentUserId4,
            FirstName = "Patrice",
            LastName = "Fouque",
            UserName = "adherent4@localhost",
            Email = "adherent4@localhost",
            Job = "Software Engineer",
            Avatar = []
        };

        await CreateUser(userManager, adherent, "Adherent1!");
        await AddUserToRoles(userManager, roleManager, roles, adherent);

        await CreateUser(userManager, adherent2, "Adherent2!");
        await AddUserToRole(userManager, roleManager, roles[0], adherent2);

        await CreateUser(userManager, adherent3, "Adherent3!");
        await AddUserToRole(userManager, roleManager, roles[0], adherent3);

        await CreateUser(userManager, adherent4, "Adherent4!");
        await AddUserToRole(userManager, roleManager, roles[0], adherent4);
    }

    private static async Task SeedSampleDataAsync(ScrumboardDbContext context)
    {
        var adherent = new AdherentDao
        {
            Id = AdherentUserId
        };

        var adherent2 = new AdherentDao
        {
            Id = AdherentUserId2
        };

        var adherent3 = new AdherentDao
        {
            Id = AdherentUserId3
        };

        var adherent4 = new AdherentDao
        {
            Id = AdherentUserId4
        };

        
        var team = new TeamDao
        {
            Name = "Developer Team",
            Adherents =
            [
                adherent,
                adherent2,
                adherent3,
                adherent4
            ],
            CreatedBy = adherent.Id
        };
        var team2 = new TeamDao
        {
            Name = "Test Team", 
            Adherents = [adherent2],
            CreatedBy = adherent2.Id
        };

        var labels = new Collection<LabelDao>
        {
            new() { Name = "Design", Colour = Colour.Violet, CreatedBy = adherent.Id },
            new() { Name = "App", Colour = Colour.Gray, CreatedBy = adherent.Id },
            new() { Name = "Feature", Colour = Colour.Red, CreatedBy = adherent.Id },
            new() { Name = "Log", Colour = Colour.Blue, CreatedBy = adherent.Id },
            new() { Name = "Documentation", Colour = Colour.Rose, CreatedBy = adherent.Id },
            new() { Name = "Persistence", Colour = Colour.Yellow, CreatedBy = adherent.Id }
        };

        var boardSettings = new Collection<BoardSettingDao>
        {
            new() { Colour = Colour.Violet, CardCoverImage = true, Subscribed = true },
            new() { Colour = Colour.Yellow, }
        };

        var cards = new Collection<CardDao>
        {
            new()
            {
                Name = "Create login page",
                Description = "Create login page with social network authentication.",
                Suscribed = false,
                DueDate = DateTime.Now,
                Position = 65536,
                Labels = [labels[0], labels[1]],
                Assignees = [adherent],
                Checklists =
                [
                    new ChecklistDao
                    {
                        Name = "Checklist",
                        ChecklistItems = new Collection<ChecklistItemDao>
                        {
                            new() { Name = "Create template for the login page", IsChecked = true, CreatedBy = adherent.Id },
                            new() { Name = "Validate template for the login page", IsChecked = false, CreatedBy = adherent.Id }
                        },
                        CreatedBy = adherent.Id
                    }
                ],
                CreatedBy = adherent.Id
            },
            new()
            {
                Name = "Change background colors",
                Description = "",
                Suscribed = false,
                DueDate = null,
                Position = 131072,
                Assignees = [],
                Labels = [labels[0]],
                CreatedBy = adherent.Id
            },
            new()
            {
                Name = "Fix splash screen bugs",
                Description = "",
                Suscribed = true,
                DueDate = new DateTime(2021, 5, 15),
                Position = 65536,
                Assignees = [],
                Labels = [labels[1]],
                CreatedBy = adherent.Id
            },
            new()
            {
                Name = "Add a notification when a user adds a comment",
                Description = "",
                Suscribed = false,
                DueDate = null,
                Position = 65536,
                Assignees = [adherent],
                Labels = [labels[2]],
                CreatedBy = adherent.Id
            },
        };

        var listboards = new Collection<ListBoardDao>
        {
            new() { Name = "Design", Position = 65536, Cards = [cards[0], cards[1]], CreatedBy = adherent.Id },
            new() { Name = "Development", Position = 131072, Cards = [cards[2]], CreatedBy = adherent.Id },
            new() { Name = "Upcoming Features", Position = 196608, Cards = [cards[3]], CreatedBy = adherent.Id },
            new() { Name = "Known Bugs", Position = 262144, CreatedBy = adherent.Id },
            new()
            {
                Name = "Backlog",
                Position = 65536,
                Cards = new Collection<CardDao>
                {
                    new()
                    {
                        Name = "Write documentation for the naming convention",
                        Description = "",
                        Suscribed = false,
                        DueDate = null,
                        Position = 65536,
                        Assignees = [adherent],
                        Labels = [labels[4]],
                        CreatedBy = adherent.Id
                    },
                    new()
                    {
                        Name = "Add Serilog for logs",
                        Description = "",
                        Suscribed = false,
                        DueDate = null,
                        Position = 131072,
                        Assignees = [],
                        Labels = new Collection<LabelDao> { labels[3] },
                        CreatedBy = adherent.Id
                    },
                },
                CreatedBy = adherent.Id
            }
        };

        var boards = new Collection<BoardDao>
        {
            new()
            {
                Name = "Scrumboard Frontend",
                Uri = "scrumboard-frontend",
                IsPinned = false,
                Team = team,
                ListBoards =
                [
                    listboards[0],
                    listboards[1],
                    listboards[2],
                    listboards[3]
                ],
                BoardSetting = boardSettings[0],
                CreatedBy = adherent.Id
            },
            new()
            {
                Name = "Scrumboard Backend",
                Uri = "scrumboard-backend",
                IsPinned = true,
                Team = team,
                ListBoards = [listboards[4]],
                BoardSetting = boardSettings[1],
                CreatedBy = adherent.Id
            }
        };

        // Seed, if necessary
        if (!await context.Boards.AnyAsync())
        {
            await context.Boards.AddRangeAsync(boards);

            await context.SaveChangesAsync();
        }
    }

    private static async Task CreateUser(UserManager<ApplicationUser> userManager, ApplicationUser user,
        string userPassword)
    {
        if (userManager.Users.All(u => u.UserName != user.UserName))
            await userManager.CreateAsync(user, userPassword);
    }

    private static async Task AddUserToRoles(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, string[] roles, ApplicationUser user)
    {
        foreach (var role in roles)
            await AddUserToRole(userManager, roleManager, role, user);
    }

    private static async Task AddUserToRole(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, string role, ApplicationUser user)
    {
        IdentityRole? identityRole;
        if (!await roleManager.RoleExistsAsync(role))
        {
            identityRole = new IdentityRole(role);

            await roleManager.CreateAsync(identityRole);
        }
        else
        {
            identityRole = await roleManager.FindByNameAsync(role);
        }

        if (!await userManager.IsInRoleAsync(user, identityRole!.Name!))
            await userManager.AddToRoleAsync(user, identityRole.Name!);
    }
}
