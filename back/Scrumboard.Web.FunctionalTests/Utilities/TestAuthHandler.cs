﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IList<Claim> _claims;

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger,
        UrlEncoder encoder, 
        TestClaimsProvider claimsProvider) 
        : base(options, logger, encoder)
    {
            _claims = claimsProvider.Claims;
        }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
            var identity = new ClaimsIdentity(_claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
}
