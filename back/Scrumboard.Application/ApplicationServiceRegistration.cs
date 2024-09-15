using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Application.Abstractions.Users;
using Scrumboard.Application.Abstractions.WeatherForecasts;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Boards.Labels;
using Scrumboard.Application.Cards;
using Scrumboard.Application.Cards.Activities;
using Scrumboard.Application.Cards.Comments;
using Scrumboard.Application.ListBoards;
using Scrumboard.Application.Teams;
using Scrumboard.Application.Users;
using Scrumboard.Application.WeatherForecasts;

namespace Scrumboard.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        
        services.AddAutoMapper(assembly);
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        // Boards
        services
            .AddScoped<IBoardsService, BoardsService>()
            .AddScoped<ILabelsService, LabelsService>();
        
        // Cards
        services
            .AddScoped<IActivityFactory, ActivityFactory>()
            .AddScoped<IActivitiesService, ActivitiesService>()
            .AddScoped<ICardsService, CardsService>()
            .AddScoped<ICommentsService, CommentsService>();
        
        // ListBoards
        services.AddScoped<IListBoardsService, ListBoardsService>();
        
        // Teams
        services.AddScoped<ITeamsService, TeamsService>();
        
        // Users
        services.AddScoped<IUsersService, UsersService>();
        
        // WeatherForecasts
        services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();
        
        return services;
    }
}
