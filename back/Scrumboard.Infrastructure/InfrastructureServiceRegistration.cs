﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.FileExport;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;
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
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards.Labels;
using Scrumboard.Infrastructure.Persistence.Cards;
using Scrumboard.Infrastructure.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Persistence.Cards.Comments;
using Scrumboard.Infrastructure.Persistence.Interceptors;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Infrastructure.Persistence.Teams;
using Scrumboard.SharedKernel.DomainEvents;

namespace Scrumboard.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
            services.AddScoped<ISaveChangesInterceptor, PublishDomainEventsInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, CreatedEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, ModifiedEntityInterceptor>();
            
            services.AddDbContext<ScrumboardDbContext>((serviceProvider, options) =>
            {
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            });
            
            
            services.AddScoped<ScrumboardDbContextInitializer>();

            services
                .AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ScrumboardDbContext>();
            
            services.AddTransient<ICurrentDateService, CurrentDateService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient(typeof(ICsvExporter<>), typeof(CsvExporter<>));
            
            // DomainEventPublisher
            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
            
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
            
            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddAuthentication();
            
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            //});

            return services;
        }
}
