using System.Security.Claims;
using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Web.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId is not null 
                ? Guid.Parse(userId) 
                : Guid.Empty;
        }
    }
}
