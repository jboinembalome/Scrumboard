using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Persistence;

public static class ScrumboardDbContextSeed
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}


public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ScrumboardDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly Guid _adminUserId = Guid.Parse("31a7ffcf-d099-4637-bd58-2a87641d1aaf");
    private readonly Guid _adherentUserId = Guid.Parse("533f27ad-d3e8-4fe7-9259-ee4ef713dbea");
    private readonly Guid _adherentUserId2 = Guid.Parse("633f27ad-d3e8-4fe7-9259-ee4ef713dbea");
    private readonly Guid _adherentUserId3 = Guid.Parse("635f27ad-d3e8-4fe7-9259-ee4ef713dbea");
    private readonly Guid _adherentUserId4 = Guid.Parse("637f27ad-d3e8-4fe7-9259-ee4ef713dbea");
    
    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ScrumboardDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        if (!_context.Database.IsSqlServer())
        {
            return;
        }
       
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
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
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        await SeedDefaultUserAsync(_userManager, _roleManager);
        await SeedSampleDataAsync(_context);
    }
    
    private async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        var roles = new[] { "Administrator", "Adherent" };
        var administrator = new ApplicationUser { Id = _adminUserId, UserName = "administrator@localhost", Email = "administrator@localhost", FirstName = "Admin", LastName = "Istrator", Job = "Administrator", Avatar = [] };
        var adherent = new ApplicationUser { Id = _adherentUserId, FirstName = "Jimmy", LastName = "Boinembalome", UserName = "adherent@localhost", Email = "adherent@localhost", Job = "Software Engineer", Avatar = []};
        var adherent2 = new ApplicationUser { Id = _adherentUserId2, FirstName = "Guyliane", LastName = "De Jesus Pimenta", UserName = "adherent2@localhost", Email = "adherent2@localhost", Job = "Software Engineer", Avatar = [] };
        var adherent3 = new ApplicationUser { Id = _adherentUserId3, FirstName = "Corentin", LastName = "Hugot", UserName = "adherent3@localhost", Email = "adherent3@localhost", Job = "Systems and Networks Engineer", Avatar = [] };
        var adherent4 = new ApplicationUser { Id = _adherentUserId4, FirstName = "Patrice", LastName = "Fouque", UserName = "adherent4@localhost", Email = "adherent4@localhost", Job = "Software Engineer", Avatar = [] };

        await CreateUser(userManager, administrator, "Administrator1!");
        await AddUserToRole(userManager, roleManager, roles[0], administrator);

        await CreateUser(userManager, adherent, "Adherent1!");
        await AddUserToRoles(userManager, roleManager, roles, adherent);

        await CreateUser(userManager, adherent2, "Adherent2!");
        await AddUserToRole(userManager, roleManager, roles[1], adherent2);

        await CreateUser(userManager, adherent3, "Adherent3!");
        await AddUserToRole(userManager, roleManager, roles[1], adherent3);

        await CreateUser(userManager, adherent4, "Adherent4!");
        await AddUserToRole(userManager, roleManager, roles[1], adherent4);
    }

    private async Task SeedSampleDataAsync(ScrumboardDbContext context)
    {
        var team = new Team
        {
            Name = "Developer Team", 
            Adherents = 
            [
                _adherentUserId, 
                _adherentUserId2, 
                _adherentUserId3, 
                _adherentUserId4
            ]
        };
        var team2 = new Team
        {
            Name = "Test Team", 
            Adherents = [_adherentUserId2]
        };

        var labels = new Collection<Label>
        {
            new Label
            {
                Name = "Design",
                Colour = Colour.Violet
            },
            new Label
            {
                Name = "App",
                Colour = Colour.Gray
            },
            new Label
            {
                Name = "Feature",
                Colour = Colour.Red
            },
            new Label
            {
                Name = "Log",
                Colour = Colour.Blue
            },
            new Label
            {
                Name = "Documentation",
                Colour = Colour.Rose
            },
            new Label
            {
                Name = "Persitence",
                Colour = Colour.Yellow
            }
        };

        var boardSettings = new Collection<BoardSetting>
        {
            new BoardSetting
            {
                Colour = Colour.Violet,
                CardCoverImage = true,
                Subscribed = true
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
                ActivityType = ActivityType.Added,
                ActivityField = ActivityField.Card,
                OldValue = string.Empty,
                NewValue = "Create login page",
            },
        };

        var card1 = new Card
        {
            Name = "Create login page",
            Description = "Create login page with social network authenfication.",
            Suscribed = false,
            DueDate = DateTime.Now,
            Position = 65536,
            Labels = [labels[0], labels[1]],
            Assignees = [_adherentUserId],
            Activities = [activities[0]],
            Checklists = 
            [
                new Checklist
                {
                    Name = "Checklist",
                    ChecklistItems = new Collection<ChecklistItem>
                    {
                        new ChecklistItem
                        {
                            Name = "Create template for the login page", IsChecked = true,
                        },
                        new ChecklistItem
                        {
                            Name = "Validate template for the login page", IsChecked = false,
                        }
                    }
                }
            ],
            Comments = 
            [
                new Comment
                {
                    Message = "The template for the login page is available on the cloud.",
                }
            ]
        };
        
        var card2 = new Card
        {
            Name = "Change background colors",
            Description = "",
            Suscribed = false,
            DueDate = null,
            Position = 131072,
            Assignees = [],
            Labels = [labels[0]],
        };
        var card3 = new Card
        {
            Name = "Fix splash screen bugs",
            Description = "",
            Suscribed = true,
            DueDate = new DateTime(2021, 5, 15),
            Position = 65536,
            Assignees = [],
            Labels = [labels[1]],
        };
        var card4 = new Card
        {
            Name = "Add a notification when a user adds a comment",
            Description = "",
            Suscribed = false,
            DueDate = null,
            Position = 65536,
            Assignees = [_adherentUserId],
            Labels = [labels[2]]
        };

        var card5 = new Card
        {
            Name = "Write documentation for the naming convention",
            Description = "",
            Suscribed = false,
            DueDate = null,
            Position = 65536,
            Assignees = [_adherentUserId],
            Labels = [labels[4]]
        };
        
        var cards = new Collection<Card>
        {
            card1,
            card2,
            card3,
            card4
        };

        var listboards = new Collection<ListBoard>
        {
            new ListBoard
            {
                Name = "Design",
                Position = 65536,
                Cards = [cards[0], cards[1]]
            },
            new ListBoard
            {
                Name = "Development",
                Position = 131072,
                Cards =  [cards[2]]
            },
            new ListBoard
            {
                Name = "Upcoming Features",
                Position = 196608,
                Cards = [cards[3]]
            },
            new ListBoard
            {
                Name = "Known Bugs",
                Position = 262144,
            },
            new ListBoard
            {
                Name = "Backlog",
                Position = 65536,
                Cards = 
                [
                    card5,
                    new Card
                    {
                        Name = "Add Serilog for logs",
                        Description = "",
                        Suscribed = false,
                        DueDate = null,
                        Position = 131072,
                        Assignees = [],
                        Labels = new Collection<Label> { labels[3] },
                    },
                ]
            }
        };

        var boards = new Collection<Board>
        {
            new Board
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
                BoardSetting = boardSettings[0]
            },
            new Board
            {
                Name = "Scrumboard Backend",
                Uri = "scrumboard-backend",
                IsPinned = true,
                Team = team,
                ListBoards = [listboards[4]],
                BoardSetting = boardSettings[1]
            },
            new Board
            {
                Name = "Scrumboard Test",
                Uri = "scrumboard-test",
                Team = team2,
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

    private static async Task CreateUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string userPassword)
    {
        if (userManager.Users.All(u => u.UserName != user.UserName))
            await userManager.CreateAsync(user, userPassword);
    }

    private static async Task AddUserToRoles(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, string[] roles, ApplicationUser user)
    {
        foreach (var role in roles)
            await AddUserToRole(userManager, roleManager, role, user);
    }

    private static async Task AddUserToRole(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, string role, ApplicationUser user)
    {
        ApplicationRole? identityRole;
        if (!await roleManager.RoleExistsAsync(role))
        {
            identityRole = new ApplicationRole(role);

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
