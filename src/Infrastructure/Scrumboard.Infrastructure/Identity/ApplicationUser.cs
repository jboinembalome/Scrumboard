using Microsoft.AspNetCore.Identity;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Infrastructure.Identity;

public class ApplicationUser : IdentityUser, IUser
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