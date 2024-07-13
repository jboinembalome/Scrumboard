using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Cards;

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
        services.AddScoped<IBoardsService, BoardsService>();
        
        // Cards
        services.AddScoped<ICardsService, CardsService>();

        return services;
    }
}
