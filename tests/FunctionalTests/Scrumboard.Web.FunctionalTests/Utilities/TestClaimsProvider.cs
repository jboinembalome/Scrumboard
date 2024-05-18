using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Scrumboard.Web.FunctionalTests.Utilities;

public class TestClaimsProvider
{
    public IList<Claim> Claims { get; }

    public TestClaimsProvider(IList<Claim> claims) => Claims = claims;

    public TestClaimsProvider() => Claims = new List<Claim>();

    public static TestClaimsProvider WithAdministratorClaims()
    {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "31a7ffcf-d099-4637-bd58-2a87641d1aaf"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "administrator@localhost"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

            return provider;
        }

    public static TestClaimsProvider WithAdherentClaims()
    {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "533f27ad-d3e8-4fe7-9259-ee4ef713dbea"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "adherent@localhost"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Adherent"));

            return provider;
        }

    public static TestClaimsProvider WithUserClaims()
    {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "user@localhost"));

            return provider;
        }
}