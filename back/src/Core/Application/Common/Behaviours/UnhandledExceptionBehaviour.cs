using MediatR;
using Scrumboard.Infrastructure.Abstractions.Logging;

namespace Scrumboard.Application.Common.Behaviours;

internal sealed class UnhandledExceptionBehaviour<TRequest, TResponse>(
    IAppLogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,  CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogError(ex, "Scrumboard API Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
