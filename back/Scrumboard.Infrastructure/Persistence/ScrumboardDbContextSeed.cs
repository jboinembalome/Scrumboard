using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
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
        await SeedSampleDataAsync();
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
            Id = _userId2.Value,
            FirstName = "Guyliane",
            LastName = "De Jesus Pimenta",
            UserName = "adherent2@localhost",
            Email = "adherent2@localhost",
            Job = "Software Engineer",
            Avatar = []
        };
        var user3 = new ApplicationUser
        {
            Id = _userId3.Value,
            FirstName = "Corentin",
            LastName = "Hugot",
            UserName = "adherent3@localhost",
            Email = "adherent3@localhost",
            Job = "Systems and Networks Engineer",
            Avatar = []
        };
        var user4 = new ApplicationUser
        {
            Id = _userId4.Value,
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

    private async Task SeedSampleDataAsync()
    {
        await SeedBoardsAsync();
    }

    private async Task<Collection<Board>> SeedBoardsAsync()
    {
        var board1 = new Board(
            name: "Scrumboard Frontend",
            isPinned: false,
            boardSetting: new BoardSetting { Colour = Colour.Violet },
            ownerId: (OwnerId)_userId.Value);
        
        var board2 = new Board(
            name: "Scrumboard Backend",
            isPinned: true,
            boardSetting: new BoardSetting { Colour = Colour.Yellow },
            ownerId: (OwnerId)_userId.Value);
        
        var boards = new Collection<Board>
        {
            board1,
            board2 
        };

        // Seed, if necessary
        if (!await context.Boards.AnyAsync())
        {
            await context.Boards.AddRangeAsync(boards);

            await context.SaveChangesAsync();
        }

        return boards;
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
