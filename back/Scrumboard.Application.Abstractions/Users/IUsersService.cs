using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Application.Abstractions.Users;

public interface IUsersService
{
    Task<IReadOnlyList<IUser>> GetAsync(CancellationToken cancellationToken = default);
    Task<byte[]> GetAvatarByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
