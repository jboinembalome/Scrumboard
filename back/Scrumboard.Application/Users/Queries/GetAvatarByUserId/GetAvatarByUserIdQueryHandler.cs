using MediatR;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Users.Queries.GetAvatarByUserId;

internal sealed class GetAvatarByUserIdQueryHandler(
    IIdentityService identityService)
    : IRequestHandler<GetAvatarByUserIdQuery, byte[]>
{
    public async Task<byte[]> Handle(
        GetAvatarByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var user = await identityService.GetUserAsync(request.UserId, cancellationToken);

        return user.Avatar;
    }
}
