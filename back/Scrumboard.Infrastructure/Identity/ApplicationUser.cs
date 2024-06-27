using Microsoft.AspNetCore.Identity;
using Scrumboard.Infrastructure.Abstractions.Identity;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>, IUser
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

public sealed class ApplicationRole : IdentityRole<Guid>
{
    /// <summary>
    /// Initializes a new instance of <see cref="IdentityRole{TKey}"/>.
    /// </summary>
    public ApplicationRole() { }

    /// <summary>
    /// Initializes a new instance of <see cref="IdentityRole{TKey}"/>.
    /// </summary>
    /// <param name="roleName">The role name.</param>
    public ApplicationRole(string roleName)
    {
        Name = roleName;
    }
}
