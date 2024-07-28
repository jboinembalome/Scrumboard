using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.Cards.Checklists;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Infrastructure.Persistence.Teams;
using Scrumboard.Infrastructure.Persistence.Boards.Labels;

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
    private const string UserId = "533f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private const string UserId2 = "633f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private const string UserId3 = "635f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private const string UserId4 = "637f27ad-d3e8-4fe7-9259-ee4ef713dbea";

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
        
        var user = new ApplicationUser
        {
            Id = UserId,
            FirstName = "Jimmy",
            LastName = "Boinembalome",
            UserName = "adherent@localhost",
            Email = "adherent@localhost",
            Job = "Software Engineer",
            Avatar = []
        };
        var user2 = new ApplicationUser
        {
            Id = UserId2,
            FirstName = "Guyliane",
            LastName = "De Jesus Pimenta",
            UserName = "adherent2@localhost",
            Email = "adherent2@localhost",
            Job = "Software Engineer",
            Avatar = []
        };
        var user3 = new ApplicationUser
        {
            Id = UserId3,
            FirstName = "Corentin",
            LastName = "Hugot",
            UserName = "adherent3@localhost",
            Email = "adherent3@localhost",
            Job = "Systems and Networks Engineer",
            Avatar = []
        };
        var user4 = new ApplicationUser
        {
            Id = UserId4,
            FirstName = "Patrice",
            LastName = "Fouque",
            UserName = "adherent4@localhost",
            Email = "adherent4@localhost",
            Job = "Software Engineer",
            Avatar = []
        };

        await CreateUser(userManager, user, "Adherent1!");
        await AddUserToRoles(userManager, roleManager, roles, user);

        await CreateUser(userManager, user2, "Adherent2!");
        await AddUserToRole(userManager, roleManager, roles[0], user2);

        await CreateUser(userManager, user3, "Adherent3!");
        await AddUserToRole(userManager, roleManager, roles[0], user3);

        await CreateUser(userManager, user4, "Adherent4!");
        await AddUserToRole(userManager, roleManager, roles[0], user4);
    }

    private static async Task SeedSampleDataAsync(ScrumboardDbContext context)
    {
        var team = new TeamDao
        {
            Name = "Developer Team",
            CreatedBy = UserId
        };

        team.Members =
        [
            new TeamMemberDao { TeamId = team.Id, MemberId = UserId, },
            new TeamMemberDao { TeamId = team.Id, MemberId = UserId2, },
            new TeamMemberDao { TeamId = team.Id, MemberId = UserId3, },
            new TeamMemberDao { TeamId = team.Id, MemberId = UserId4, }
        ];
        
        var team2 = new TeamDao
        {
            Name = "Test Team", 
            CreatedBy = UserId2
        };
        
        team2.Members =
        [
            new TeamMemberDao { TeamId = team2.Id, MemberId = UserId2 },
        ];

        var labels = new Collection<LabelDao>
        {
            new() { Name = "Design", Colour = Colour.Violet, CreatedBy = UserId },
            new() { Name = "App", Colour = Colour.Gray, CreatedBy = UserId },
            new() { Name = "Feature", Colour = Colour.Red, CreatedBy = UserId },
            new() { Name = "Log", Colour = Colour.Blue, CreatedBy = UserId },
            new() { Name = "Documentation", Colour = Colour.Rose, CreatedBy = UserId },
            new() { Name = "Persistence", Colour = Colour.Yellow, CreatedBy = UserId }
        };

        var boardSettings = new Collection<BoardSettingDao>
        {
            new() { Colour = Colour.Violet, CardCoverImage = true, Subscribed = true },
            new() { Colour = Colour.Yellow, }
        };

        CardDao card1 = new()
        {
            Name = "Create login page",
            Description = "Create login page with social network authentication.",
            Suscribed = false,
            DueDate = DateTime.Now,
            Position = 65536,
            Labels = [labels[0], labels[1]],
            Checklists =
            [
                new ChecklistDao
                {
                    Name = "Checklist",
                    ChecklistItems = new Collection<ChecklistItemDao>
                    {
                        new() { Name = "Create template for the login page", IsChecked = true, CreatedBy = UserId },
                        new() { Name = "Validate template for the login page", IsChecked = false, CreatedBy = UserId }
                    },
                    CreatedBy = UserId
                }
            ],
            CreatedBy = UserId
        };

        card1.Assignees = 
        [
            new CardAssigneeDao { CardId = card1.Id, AssigneeId = UserId }
        ];
        
        CardDao card2 = new()
        {
            Name = "Change background colors",
            Description = "",
            Suscribed = false,
            DueDate = null,
            Position = 131072,
            Labels = [labels[0]],
            CreatedBy = UserId
        };
        CardDao card3 = new()
        {
            Name = "Fix splash screen bugs",
            Description = "",
            Suscribed = true,
            DueDate = new DateTime(2021, 5, 15),
            Position = 65536,
            Labels = [labels[1]],
            CreatedBy = UserId
        };
        CardDao card4 = new()
        {
            Name = "Add a notification when a user adds a comment",
            Description = "",
            Suscribed = false,
            DueDate = null,
            Position = 65536,
            Labels = [labels[2]],
            CreatedBy = UserId
        };
        
        card4.Assignees = 
        [
            new CardAssigneeDao { CardId = card4.Id, AssigneeId = UserId }
        ];
        
        var cards = new Collection<CardDao>
        {
            card1,
            card2,
            card3,
            card4,
        };

        CardDao card5 = new()
        {
            Name = "Write documentation for the naming convention",
            Description = "",
            Suscribed = false,
            DueDate = null,
            Position = 65536,
            Labels = [labels[4]],
            CreatedBy = UserId
        };
        
        card5.Assignees = 
        [
            new CardAssigneeDao { CardId = card5.Id, AssigneeId = UserId }
        ];
        
        CardDao card6 = new()
        {
            Name = "Add Serilog for logs",
            Description = "",
            Suscribed = false,
            DueDate = null,
            Position = 131072,
            Assignees = [],
            Labels = new Collection<LabelDao> { labels[3] },
            CreatedBy = UserId
        };
        var listboards = new Collection<ListBoardDao>
        {
            new() { Name = "Design", Position = 65536, Cards = [cards[0], cards[1]], CreatedBy = UserId },
            new() { Name = "Development", Position = 131072, Cards = [cards[2]], CreatedBy = UserId },
            new() { Name = "Upcoming Features", Position = 196608, Cards = [cards[3]], CreatedBy = UserId },
            new() { Name = "Known Bugs", Position = 262144, CreatedBy = UserId },
            new()
            {
                Name = "Backlog",
                Position = 65536,
                Cards = new Collection<CardDao>
                {
                    card5,
                    card6,
                },
                CreatedBy = UserId
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
                CreatedBy = UserId
            },
            new()
            {
                Name = "Scrumboard Backend",
                Uri = "scrumboard-backend",
                IsPinned = true,
                Team = team,
                ListBoards = [listboards[4]],
                BoardSetting = boardSettings[1],
                CreatedBy = UserId
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
