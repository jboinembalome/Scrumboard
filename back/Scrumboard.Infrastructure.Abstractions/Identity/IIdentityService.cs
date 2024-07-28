using Scrumboard.Domain.Common;

namespace Scrumboard.Infrastructure.Abstractions.Identity;

public interface IIdentityService
{
    Task<IUser?> TryGetUserAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<IUser> GetUserAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<string?> GetUserNameAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IUser>> GetListAllAsync(CancellationToken cancellationToken = default);

    Task<IList<IUser>> GetUsersInRoleAsync(string role);

    Task<IReadOnlyList<IUser>> GetListAsync(IEnumerable<UserId> userIds, CancellationToken cancellationToken = default);

    Task<bool> IsInRoleAsync(UserId userId, string role);

    Task AddUserToRolesAsync(UserId userId, IEnumerable<string> roles);

    Task AddUserToRoleAsync(UserId userId, string role);

    Task<bool> AuthorizeAsync(UserId userId, string policyName);

    Task<(Result Result, UserId UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(UserId userId);
}
