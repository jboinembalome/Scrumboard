using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal static class WebApplicationFactoryExtensions
{
    public static HttpClient CreateUserClient<T>(this WebApplicationFactory<T> factory,
        TestUser testUser) where T : class
    {
        var claimsProvider = GetClaimsProvider(testUser);

        var client = factory.WithAuthentication(claimsProvider)
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        return client;
    }

    private static TestClaimsProvider GetClaimsProvider(TestUser testUser)
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, testUser.Id));
        provider.Claims.Add(new Claim(ClaimTypes.Name, testUser.UserName));
        provider.Claims.Add(new Claim(ClaimTypes.Role, string.Join(',', testUser.Roles)));
        
        return provider;
    }

    private static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory,
        TestClaimsProvider claimsProvider) where T : class 
        => factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", op => { });

                services.AddScoped(_ => claimsProvider);
            });
        });
}
