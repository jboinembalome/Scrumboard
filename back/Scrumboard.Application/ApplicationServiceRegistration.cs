using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Cards;
using Scrumboard.Application.Common.Behaviours;

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
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
        
        // Boards
        services.AddScoped<IBoardsService, BoardsService>();
        
        // Cards
        services.AddScoped<ICardsService, CardsService>();

        return services;
    }
}
