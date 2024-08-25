using System.Security.Claims;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal sealed class ClaimsProvider(IList<Claim> claims)
{
    public IList<Claim> Claims { get; } = claims;

    public ClaimsProvider() : this(new List<Claim>())
    {
    }
    
    public static ClaimsProvider GetClaims(TestUser testUser)
    {
        var provider = new ClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, testUser.Id));
        provider.Claims.Add(new Claim(ClaimTypes.Name, testUser.UserName));
        
        foreach (var role in testUser.Roles)
        {
            provider.Claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        return provider;
    }
}
