using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.SharedKernel.Types;

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
    private readonly UserId _userId = (UserId)"533f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private readonly UserId _userId2 = (UserId)"633f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private readonly UserId _userId3 = (UserId)"635f27ad-d3e8-4fe7-9259-ee4ef713dbea";
    private readonly UserId _userId4 = (UserId)"637f27ad-d3e8-4fe7-9259-ee4ef713dbea";

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

    private async Task SeedDefaultUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Adherent" };
        
        var user = new ApplicationUser
        {
            Id = _userId.Value,
            FirstName = "Jimmy",
            LastName = "Boinembalome",
            UserName = "adherent@localhost",
            Email = "adherent@localhost",
            Job = "Software Engineer",
            Avatar = []
        };
        var user2 = new ApplicationUser
        {
            Id = _userId2,
            FirstName = "Guyliane",
            LastName = "De Jesus Pimenta",
            UserName = "adherent2@localhost",
            Email = "adherent2@localhost",
            Job = "Software Engineer",
            Avatar = []
        };
        var user3 = new ApplicationUser
        {
            Id = _userId3,
            FirstName = "Corentin",
            LastName = "Hugot",
            UserName = "adherent3@localhost",
            Email = "adherent3@localhost",
            Job = "Systems and Networks Engineer",
            Avatar = []
        };
        var user4 = new ApplicationUser
        {
            Id = _userId4,
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

    private async Task SeedSampleDataAsync(ScrumboardDbContext context)
    {
        var team = new Team
        {
            Name = "Developer Team",
            CreatedBy = _userId
        };

        team.AddMember((MemberId)_userId.Value);
        team.AddMember((MemberId)_userId2.Value);
        team.AddMember((MemberId)_userId3.Value);
        team.AddMember((MemberId)_userId4.Value);
        
        var team2 = new Team
        {
            Name = "Test Team", 
            CreatedBy = _userId2
        };
        
        team2.AddMember((MemberId)_userId2.Value);

        var labels = new Collection<Label>
        {
            new() { Name = "Design", Colour = Colour.Violet, CreatedBy = (UserId)_userId },
            new() { Name = "App", Colour = Colour.Gray, CreatedBy = (UserId)_userId },
            new() { Name = "Feature", Colour = Colour.Red, CreatedBy = (UserId)_userId },
            new() { Name = "Log", Colour = Colour.Blue, CreatedBy = (UserId)_userId },
            new() { Name = "Documentation", Colour = Colour.Rose, CreatedBy = (UserId)_userId },
            new() { Name = "Persistence", Colour = Colour.Yellow, CreatedBy = (UserId)_userId }
        };

        var boardSettings = new Collection<BoardSetting>
        {
            new() { Colour = Colour.Violet },
            new() { Colour = Colour.Yellow }
        };

        Card card1 = new()
        {
            Name = "Create login page",
            Description = "Create login page with social network authentication.",
            DueDate = DateTime.Now,
            Position = 65536,
            CreatedBy = _userId
        };
        
        card1.AddLabel(labels[0].Id);
        card1.AddLabel(labels[1].Id);

        card1.AddAssignee((AssigneeId)_userId.Value);
        
        Card card2 = new()
        {
            Name = "Change background colors",
            Description = "",
            DueDate = null,
            Position = 131072,
            CreatedBy = _userId
        };
        
        card2.AddLabel(labels[0].Id);
        
        Card card3 = new()
        {
            Name = "Fix splash screen bugs",
            Description = "",
            DueDate = new DateTime(2021, 5, 15),
            Position = 65536,
            CreatedBy = _userId
        };
        
        card3.AddLabel(labels[1].Id);
        
        Card card4 = new()
        {
            Name = "Add a notification when a user adds a comment",
            Description = "",
            DueDate = null,
            Position = 65536,
            CreatedBy = _userId
        };

        card3.AddLabel(labels[2].Id);
        
        card4.AddAssignee((AssigneeId)_userId.Value);
        
        var cards = new Collection<Card>
        {
            card1,
            card2,
            card3,
            card4,
        };

        Card card5 = new()
        {
            Name = "Write documentation for the naming convention",
            Description = "",
            DueDate = null,
            Position = 65536,
            CreatedBy = _userId
        };
        
        card5.AddLabel(labels[4].Id);
        card5.AddAssignee((AssigneeId)_userId.Value);
        
        Card card6 = new()
        {
            Name = "Add Serilog for logs",
            Description = "",
            DueDate = null,
            Position = 131072,
            CreatedBy = _userId
        };
        
        card6.AddLabel(labels[3].Id);
        
        
        var listboards = new Collection<ListBoard>
        {
            new() { Name = "Design", Position = 65536, Cards = [cards[0], cards[1]], CreatedBy = _userId },
            new() { Name = "Development", Position = 131072, Cards = [cards[2]], CreatedBy = _userId },
            new() { Name = "Upcoming Features", Position = 196608, Cards = [cards[3]], CreatedBy = _userId },
            new() { Name = "Known Bugs", Position = 262144, CreatedBy = _userId },
            new()
            {
                Name = "Backlog",
                Position = 65536,
                Cards = new Collection<Card>
                {
                    card5,
                    card6,
                },
                CreatedBy = _userId
            }
        };

        var boards = new Collection<Board>
        {
            new()
            {
                Name = "Scrumboard Frontend",
                IsPinned = false,
                BoardSetting = boardSettings[0],
                CreatedBy = (UserId)_userId
            },
            new()
            {
                Name = "Scrumboard Backend",
                IsPinned = true,
                BoardSetting = boardSettings[1],
                CreatedBy = (UserId)_userId
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
