using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Infrastructure.Identity;

internal sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService)
    : IIdentityService
{
    public async Task<IUser?> TryGetUserAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }
    public async Task<IUser> GetUserAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<string?> GetUserNameAsync(
        Guid userId, 
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
        IEnumerable<Guid> userIds, 
        CancellationToken cancellationToken = default)
    {
        var users = await userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

        return user is not null && await userManager.IsInRoleAsync(user, role);
    }

    public async Task AddUserToRolesAsync(Guid userId, IEnumerable<string> roles)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        foreach (var role in roles)
            if (user is not null)
            {
                await AddUserToRole(user, role);
            }
    }

    public async Task AddUserToRoleAsync(Guid userId, string role)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is not null)
        {
            await AddUserToRole(user, role);
        }
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
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

    public async Task<Result> DeleteUserAsync(Guid userId)
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
        ApplicationRole? identityRole;
        if (!await roleManager.RoleExistsAsync(role))
        {
            identityRole = new ApplicationRole(role);

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
