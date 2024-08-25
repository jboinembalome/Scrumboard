using Scrumboard.Infrastructure.Identity;

namespace Scrumboard.Web.Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddAuthentication();
            
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ApplicationAccess, policy => 
                policy.RequireRole(Roles.ApplicationAccess));
            
            var applicationAccessPolicy = options.GetPolicy(Policies.ApplicationAccess) 
                                          ?? throw new InvalidOperationException($"Required policy {Policies.ApplicationAccess} not found.");
            
            options.DefaultPolicy = applicationAccessPolicy;
            options.FallbackPolicy = applicationAccessPolicy;
        });

        return services;
    }
}
