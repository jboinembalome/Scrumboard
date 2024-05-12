using MediatR;

namespace Scrumboard.Application.Adherents.GetAvatarByIdentityId;

public class GetAvatarByIdentityIdQuery : IRequest<byte[]>
{
    public string IdentityId { get; set; }
}