using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scrumboard.Infrastructure.Identity;

internal sealed class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileService(
        UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        ApplicationUser user = await _userManager.GetUserAsync(context.Subject);

        IList<string> roles = await _userManager.GetRolesAsync(user);
        
        var current = _httpContextAccessor.HttpContext;
        
        var avatarPath = user.Avatar is not null 
            ? $"{current.Request.Scheme}://{current.Request.Host}{current.Request.PathBase}/api/adherents/avatar/{user.Id}" 
            : string.Empty;
        
        var claims = new List<Claim> {
            // Here you can include other properties such as id, email, address, etc. as part of the jwt claim types
            new(JwtClaimTypes.Id, user.Id),
            new(JwtClaimTypes.Email, user.Email),
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