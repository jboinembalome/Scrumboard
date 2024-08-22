using Scrumboard.Application.Abstractions.Users;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Application.Users;

internal sealed class UsersService(
    IIdentityService identityService) : IUsersService
{
    public Task<IReadOnlyList<IUser>> GetAsync(
        CancellationToken cancellationToken = default) 
        => identityService.GetListAllAsync(cancellationToken);

    public async Task<byte[]> GetAvatarByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserAsync(userId, cancellationToken);

        return user.Avatar;
    }
}
