using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scrumboard.Infrastructure.Abstractions.Logging;

namespace Scrumboard.Application.Common.Behaviours;

internal sealed class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IAppLogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(IAppLogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "Scrumboard API Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}