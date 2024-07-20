﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Application.Abstractions.Users;
using Scrumboard.Application.Abstractions.WeatherForecasts;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Boards.Labels;
using Scrumboard.Application.Cards;
using Scrumboard.Application.Cards.Activities;
using Scrumboard.Application.Cards.Comments;
using Scrumboard.Application.Teams;
using Scrumboard.Application.Users;
using Scrumboard.Application.WeatherForecasts;

namespace Scrumboard.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            // Configuration code
            cfg.AddCollectionMappers();
        }, Assembly.GetExecutingAssembly());
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        // Boards
        services
            .AddScoped<IBoardsService, BoardsService>()
            .AddScoped<ILabelsService, LabelsService>();
        
        // Cards
        services
            .AddScoped<IActivitiesService, ActivitiesService>()
            .AddScoped<ICardsService, CardsService>()
            .AddScoped<ICommentsService, CommentsService>();
        
        // Teams
        services.AddScoped<ITeamsService, TeamsService>();
        
        // Users
        services.AddScoped<IUsersService, UsersService>();
        
        // WeatherForecasts
        services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();
        
        return services;
    }
}
