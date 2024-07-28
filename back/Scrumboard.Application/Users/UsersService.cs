using Scrumboard.Application.Abstractions.Users;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Users;

internal sealed class UsersService(
    IIdentityService identityService) : IUsersService
{
    public Task<IReadOnlyList<IUser>> GetAsync(CancellationToken cancellationToken = default) 
        => identityService.GetListAllAsync(cancellationToken);

    public async Task<byte[]> GetAvatarByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserAsync(userId, cancellationToken);

        return user.Avatar;
    }
}
