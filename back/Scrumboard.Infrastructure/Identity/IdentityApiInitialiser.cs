using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Scrumboard.Infrastructure.Identity;

public static class IdentityApiInitialiser
{
    public static WebApplication InitialiseIdentityApi(this WebApplication app)
    {
        app
            .MapGroup("/api/account")
            .MapIdentityApi<ApplicationUser>();

        return app;
    }
}
