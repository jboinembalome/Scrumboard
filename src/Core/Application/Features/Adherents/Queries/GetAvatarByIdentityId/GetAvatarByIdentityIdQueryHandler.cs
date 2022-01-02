using MediatR;
using Scrumboard.Application.Interfaces.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Adherents.Queries.GetAvatarByIdentityId
{
    public class GetAvatarByIdentityIdQueryHandler : IRequestHandler<GetAvatarByIdentityIdQuery, byte[]>
    {
        private readonly IIdentityService _identityService;

        public GetAvatarByIdentityIdQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<byte[]> Handle(GetAvatarByIdentityIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserAsync(request.IdentityId, cancellationToken);

            return user.Avatar;
        }
    }
}
