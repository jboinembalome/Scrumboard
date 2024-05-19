using MediatR;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Adherents.Queries.GetAvatarByIdentityId;

internal sealed class GetAvatarByIdentityIdQueryHandler : IRequestHandler<GetAvatarByIdentityIdQuery, byte[]>
{
    private readonly IIdentityService _identityService;

    public GetAvatarByIdentityIdQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<byte[]> Handle(
        GetAvatarByIdentityIdQuery request, 
        CancellationToken cancellationToken)
    {
        // TODO: Use ICurrentUserService in infra directly
        var user = await _identityService.GetUserAsync(request.IdentityId, cancellationToken);

        return user.Avatar;
    }
}
