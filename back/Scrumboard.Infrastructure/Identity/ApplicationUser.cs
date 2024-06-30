using Microsoft.AspNetCore.Identity;
using Scrumboard.Infrastructure.Abstractions.Identity;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser, IUser
{
    [PersonalData]
    public string FirstName { get; set; }

    [PersonalData]
    public string LastName { get; set; }

    [PersonalData]
    public string Job { get; set; }

    [PersonalData]
    public byte[] Avatar { get; set; }
}
