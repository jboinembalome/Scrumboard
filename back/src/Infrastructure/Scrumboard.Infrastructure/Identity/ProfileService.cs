using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;

namespace Scrumboard.Infrastructure.Identity;

internal sealed class ProfileService(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor)
    : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await userManager.GetUserAsync(context.Subject);

        IList<string> roles = await userManager.GetRolesAsync(user!);
        
        var current = httpContextAccessor.HttpContext;
        
        var avatarPath = $"{current?.Request.Scheme}://{current?.Request.Host}{current?.Request.PathBase}/api/adherents/avatar/{user?.Id}";
        
        var claims = new List<Claim> {
            // Here you can include other properties such as id, email, address, etc. as part of the jwt claim types
            new(JwtClaimTypes.Id, user!.Id),
            new(JwtClaimTypes.Email, user.Email!),
            new(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(JwtClaimTypes.Picture, avatarPath)
        };
        
        claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

        context.IssuedClaims.AddRange(claims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}
