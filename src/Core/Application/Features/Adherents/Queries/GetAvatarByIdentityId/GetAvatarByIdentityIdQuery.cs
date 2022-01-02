using MediatR;

namespace Scrumboard.Application.Features.Adherents.Queries.GetAvatarByIdentityId
{
    public class GetAvatarByIdentityIdQuery : IRequest<byte[]>
    {
        public string IdentityId { get; set; }
    }
}
