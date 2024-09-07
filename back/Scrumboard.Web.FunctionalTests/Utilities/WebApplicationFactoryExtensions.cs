using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal static class WebApplicationFactoryExtensions
{
    public static HttpClient CreateUserClient<T>(this WebApplicationFactory<T> factory,
        TestUser testUser) where T : class
    {
        var claimsProvider = ClaimsProvider.GetClaims(testUser);

        var client = factory.WithAuthentication(claimsProvider)
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        return client;
    }

    private static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory,
        ClaimsProvider claimsProvider) where T : class 
        => factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("Test", op => { });

                services.AddScoped(_ => claimsProvider);
            });
        });
}
