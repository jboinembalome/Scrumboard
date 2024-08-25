using Scrumboard.Infrastructure.Identity;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal static class SeededUser
{
    // TODO: Use data from ScrumboardDbContextSeed
    public static readonly TestUser Adherent = new(
        Id: "533f27ad-d3e8-4fe7-9259-ee4ef713dbea",
        UserName: "adherent@localhost",
        Roles: [Roles.ApplicationAccess ,Roles.Adherent]);
    
    public static readonly TestUser NoRightUser = new(
        Id: "6349d88d-cec3-49d7-9824-7e274f1c749b",
        UserName: "no-right-user@localhost",
        Roles: []);
}

public record TestUser(string Id, string UserName, string[] Roles);
