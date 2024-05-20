using MediatR.Pipeline;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Logging;

namespace Scrumboard.Application.Common.Behaviours;

internal sealed class LoggingBehaviour<TRequest>(
    IAppLogger<TRequest> logger,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = currentUserService.UserId ?? string.Empty;
        var userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
            userName = await identityService.GetUserNameAsync(userId, cancellationToken);

        logger.LogInformation("Scrumboard API Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName!, request);
    }
}
