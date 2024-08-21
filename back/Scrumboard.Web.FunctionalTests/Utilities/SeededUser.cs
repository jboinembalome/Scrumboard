namespace Scrumboard.Web.FunctionalTests.Utilities;

internal static class SeededUser
{
    // TODO: Use data from ScrumboardDbContextSeed
    public static readonly TestUser Adherent = new(
        Id: "533f27ad-d3e8-4fe7-9259-ee4ef713dbea",
        UserName: "adherent@localhost",
        Roles: ["Adherent"]);
}

public record TestUser(string Id, string UserName, string[] Roles);
