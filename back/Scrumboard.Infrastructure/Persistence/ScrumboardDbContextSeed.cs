using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Identity;
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
    private readonly UserId _userId = "533f27ad-d3e8-4fe7-9259-ee4ef713dbea";

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
        await SeedDefaultUserAsync();
        await SeedSampleDataAsync();
    }

    private async Task SeedDefaultUserAsync()
    {
        var roles = new[] { Roles.ApplicationAccess, Roles.Adherent };
        
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

        await CreateUser(userManager, user, "Adherent1!");
        await AddUserToRoles(userManager, roleManager, roles, user);
    }

    private async Task SeedSampleDataAsync()
    {
        var boards = await SeedBoardsAsync();
        await SeedTeamsAsync(boards[0], boards[1]);
        await SeedLabelsAsync(boards[0]);
        await SeedListBoardsAsync(boards[0]);
    }

    private async Task<Collection<Board>> SeedBoardsAsync()
    {
        var board1 = new Board(
            name: "Blouppy",
            isPinned: false,
            boardSetting: new BoardSetting(
                colour:Colour.Violet),
            ownerId: _userId.Value)
        {
            CreatedBy = _userId.Value
        };

        var board2 = new Board(
            name: "Flouppy",
            isPinned: true,
            boardSetting: new BoardSetting(
                colour:Colour.Yellow),
            ownerId: _userId.Value)
        {
            CreatedBy = _userId.Value
        };

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
    
    private async Task<Collection<Team>> SeedTeamsAsync(Board board1, Board board2)
    {
        var team1 = new Team(
            name: "Blouppy team",
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        team1.AddMembers([_userId.Value]);
        
        var team2 = new Team(
            name: "Flouppy team",
            boardId: board2.Id)
        {
            CreatedBy = _userId.Value
        };
        
        team2.AddMembers([_userId.Value]);
        
        var teams = new Collection<Team>
        {
            team1,
            team2 
        };

        // Seed, if necessary
        if (!await context.Teams.AnyAsync())
        {
            await context.Teams.AddRangeAsync(teams);

            await context.SaveChangesAsync();
        }

        return teams;
    }
    
    private async Task<Collection<Label>> SeedLabelsAsync(Board board1)
    {
        var label1 = new Label(
            name: "Front",
            colour: Colour.Blue, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        
        var label2 = new Label(
            name: "Back",
            colour: Colour.Yellow, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        var labels = new Collection<Label>
        {
            label1,
            label2 
        };

        // Seed, if necessary
        if (!await context.Labels.AnyAsync())
        {
            await context.Labels.AddRangeAsync(labels);

            await context.SaveChangesAsync();
        }

        return labels;
    }
    
    private async Task<Collection<ListBoard>> SeedListBoardsAsync(Board board1)
    {
        var listBoard1 = new ListBoard(
            name: "Backlog",
            position: 1, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        var listBoard2 = new ListBoard(
            name: "Todo",
            position: 2, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        var listBoard3 = new ListBoard(
            name: "In Progress",
            position: 3, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        var listBoard4 = new ListBoard(
            name: "In Review",
            position: 4, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        var listBoard5 = new ListBoard(
            name: "In Functional Review",
            position: 5, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };

        var listBoard6 = new ListBoard(
            name: "Done",
            position: 6, 
            boardId: board1.Id)
        {
            CreatedBy = _userId.Value
        };
        
        var listBoards = new Collection<ListBoard>
        {
            listBoard1,
            listBoard2,
            listBoard3,
            listBoard4,
            listBoard5,
            listBoard6
        };

        // Seed, if necessary
        if (!await context.ListBoards.AnyAsync())
        {
            await context.ListBoards.AddRangeAsync(listBoards);

            await context.SaveChangesAsync();
        }

        return listBoards;
    }

    // TODO: Add Cards
    // TODO: Add Activities
    // TODO: Add Comments
    private static async Task CreateUser(
        UserManager<ApplicationUser> userManager, 
        ApplicationUser user,
        string userPassword)
    {
        if (userManager.Users.All(u => u.UserName != user.UserName))
            await userManager.CreateAsync(user, userPassword);
    }

    private static async Task AddUserToRoles(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        string[] roles, 
        ApplicationUser user)
    {
        foreach (var role in roles)
            await AddUserToRole(userManager, roleManager, role, user);
    }

    private static async Task AddUserToRole(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        string role, 
        ApplicationUser user)
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
