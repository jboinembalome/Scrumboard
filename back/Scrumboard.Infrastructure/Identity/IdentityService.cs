using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Infrastructure.Identity;

internal sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService)
    : IIdentityService
{
    public async Task<IUser?> TryGetUserAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }
    public async Task<IUser> GetUserAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<string?> GetUserNameAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId, cancellationToken);

        return user.UserName;
    }

    public async Task<IReadOnlyList<IUser>> GetListAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await userManager.Users.ToListAsync(cancellationToken);

        return users;
    }

    public async Task<IList<IUser>> GetUsersInRoleAsync(string role)
    {
        var users = await userManager.GetUsersInRoleAsync(role);

        return (IList<IUser>)users;
    }

    public async Task<IReadOnlyList<IUser>> GetListAsync(
        IEnumerable<string> userIds, 
        CancellationToken cancellationToken = default)
    {
        var users = await userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

        return user is not null && await userManager.IsInRoleAsync(user, role);
    }

    public async Task AddUserToRolesAsync(string userId, IEnumerable<string> roles)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        foreach (var role in roles)
            if (user is not null)
            {
                await AddUserToRole(user, role);
            }
    }

    public async Task AddUserToRoleAsync(string userId, string role)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
        {
            await AddUserToRole(user, role);
        }
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
        {
            var principal = await userClaimsPrincipalFactory.CreateAsync(user);

            var result = await authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        return false;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

        if (user is not null)
            return await DeleteUserAsync(user);

        return Result.Success();
    }

    private async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    private async Task AddUserToRole(ApplicationUser user, string role)
    {
        IdentityRole? identityRole;
        if (!await roleManager.RoleExistsAsync(role))
        {
            identityRole = new IdentityRole(role);

            await roleManager.CreateAsync(identityRole);
        }
        else
        {
            identityRole = await roleManager.FindByNameAsync(role);
        }

        if (!await userManager.IsInRoleAsync(user, identityRole!.Name!))
            await userManager.AddToRoleAsync(user, identityRole.Name!);
    }

}
