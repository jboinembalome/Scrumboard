using Scrumboard.Application.Common.Models;

namespace Scrumboard.Infrastructure.Abstractions.Identity;

public interface IIdentityService
{
    Task<IUser> GetUserAsync(string userId, CancellationToken cancellationToken = default);

    Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IUser>> GetListAllAsync(CancellationToken cancellationToken = default);

    Task<IList<IUser>> GetUsersInRoleAsync(string role);

    Task<IReadOnlyList<IUser>> GetListAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task AddUserToRolesAsync(string userId, IEnumerable<string> roles);

    Task AddUserToRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
