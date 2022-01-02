using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Scrumboard.Infrastructure.Identity
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> UserManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            UserManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ApplicationUser user = await UserManager.GetUserAsync(context.Subject);

            IList<string> roles = await UserManager.GetRolesAsync(user);
            var current = _httpContextAccessor.HttpContext;
            var avatarPath = user.Avatar != null 
                ? $"{current.Request.Scheme}://{current.Request.Host}{current.Request.PathBase}/api/adherents/avatar/{user.Id}" 
                : string.Empty;
            var claims = new List<Claim> {
                // Here you can include other properties such as id, email, address, etc. as part of the jwt claim types
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtClaimTypes.Picture, avatarPath)
            };
            foreach (string role in roles)
            {
                // Include the roles
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
