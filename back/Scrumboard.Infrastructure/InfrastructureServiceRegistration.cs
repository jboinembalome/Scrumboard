using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.FileExport;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Logging;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.Infrastructure.Common;
using Scrumboard.Infrastructure.FileExport;
using Scrumboard.Infrastructure.Identity;
using Scrumboard.Infrastructure.Logging;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards.Labels;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Persistence.Cards.Comments;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Infrastructure.Persistence.Teams;

namespace Scrumboard.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
            services.AddDbContext<ScrumboardDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            
            services.AddScoped<ScrumboardDbContextInitializer>();

            services
                .AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ScrumboardDbContext>();
            
            services.AddAutoMapper(cfg =>
            {
                // Configuration code
            }, Assembly.GetExecutingAssembly());
        
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient(typeof(ICsvExporter<>), typeof(CsvExporter<>));

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            
            
            // Boards
            services.AddScoped<IBoardsRepository, BoardsRepository>();
            services.AddScoped<IBoardsQueryRepository, BoardsQueryRepository>();
            
            // Cards
            services.AddScoped<ICardsQueryRepository, CardsQueryRepository>();
            services.AddScoped<ICardsRepository, CardsRepository>();
            services.AddScoped<IActivitiesQueryRepository, ActivitiesQueryRepository>();
            services.AddScoped<IActivitiesRepository, ActivitiesRepository>();
            services.AddScoped<ICommentsQueryRepository, CommentsQueryRepository>();
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<ILabelsQueryRepository, LabelsQueryRepository>();
            services.AddScoped<ILabelsRepository, LabelsRepository>();
            
            // ListBoards
            services.AddScoped<IListBoardsQueryRepository, ListBoardsQueryRepository>();
            services.AddScoped<IListBoardsRepository, ListBoardsRepository>();
            
            // Teams
            services.AddScoped<ITeamsQueryRepository, TeamsQueryRepository>();
            services.AddScoped<ITeamsRepository, TeamsRepository>();
           
            
            services.AddAuthentication();
            
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            //});

            return services;
        }
}
