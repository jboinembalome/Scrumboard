namespace Scrumboard.Infrastructure.Abstractions.Identity;

public interface IIdentityService
{
    Task<IUser?> TryGetUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IUser> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<string?> GetUserNameAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IUser>> GetListAllAsync(CancellationToken cancellationToken = default);

    Task<IList<IUser>> GetUsersInRoleAsync(string role);

    Task<IReadOnlyList<IUser>> GetListAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task AddUserToRolesAsync(Guid userId, IEnumerable<string> roles);

    Task AddUserToRoleAsync(Guid userId, string role);

    Task<bool> AuthorizeAsync(Guid userId, string policyName);

    Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(Guid userId);
}
