using System.Diagnostics;
using MediatR;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Logging;

namespace Scrumboard.Application.Common.Behaviours;

internal sealed class PerformanceBehaviour<TRequest, TResponse>(
    IAppLogger<TRequest> logger,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = currentUserService.UserId;
            var userName = await identityService.GetUserNameAsync(userId);

            logger.LogWarning("Scrumboard API Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName!, request);
        }

        return response;
    }
}
