using MediatR;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Adherents.Queries.GetAvatarByIdentityId;

internal sealed class GetAvatarByIdentityIdQueryHandler(IIdentityService identityService)
    : IRequestHandler<GetAvatarByIdentityIdQuery, byte[]>
{
    public async Task<byte[]> Handle(
        GetAvatarByIdentityIdQuery request, 
        CancellationToken cancellationToken)
    {
        // TODO: Use ICurrentUserService in infra directly
        var user = await identityService.GetUserAsync(request.IdentityId, cancellationToken);

        return user.Avatar;
    }
}
