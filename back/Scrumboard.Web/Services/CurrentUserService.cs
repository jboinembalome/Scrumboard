using System.Security.Claims;
using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Web.Services;

internal sealed class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
