using System.Security.Claims;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal sealed class TestClaimsProvider(IList<Claim> claims)
{
    public IList<Claim> Claims { get; } = claims;

    public TestClaimsProvider() : this(new List<Claim>())
    {
    }
}
