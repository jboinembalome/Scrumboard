using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Application.Common.Models;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Infrastructure.Identity;

internal sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<IUser> GetUserAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<string?> GetUserNameAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId, cancellationToken);

        return user.UserName;
    }

    public async Task<IReadOnlyList<IUser>> GetListAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);

        return users;
    }

    public async Task<IList<IUser>> GetUsersInRoleAsync(string role)
    {
        var users = await _userManager.GetUsersInRoleAsync(role);

        return (IList<IUser>)users;
    }

    public async Task<IReadOnlyList<IUser>> GetListAsync(
        IEnumerable<string> userIds, 
        CancellationToken cancellationToken = default)
    {
        var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync(cancellationToken);

        return users;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task AddUserToRolesAsync(string userId, IEnumerable<string> roles)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        foreach (var role in roles)
            if (user != null)
            {
                await AddUserToRole(user, role);
            }
    }

    public async Task AddUserToRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user != null)
        {
            await AddUserToRole(user, role);
        }
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user != null)
        {
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        return false;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user != null)
            return await DeleteUserAsync(user);

        return Result.Success();
    }

    private async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    private async Task AddUserToRole(ApplicationUser user, string role)
    {
        IdentityRole? identityRole;
        if (!await _roleManager.RoleExistsAsync(role))
        {
            identityRole = new IdentityRole(role);

            await _roleManager.CreateAsync(identityRole);
        }
        else
        {
            identityRole = await _roleManager.FindByNameAsync(role);
        }

        if (!await _userManager.IsInRoleAsync(user, identityRole!.Name!))
            await _userManager.AddToRoleAsync(user, identityRole.Name!);
    }

}
