using MediatR;

namespace Scrumboard.Application.Adherents.Queries.GetAvatarByIdentityId;

public sealed class GetAvatarByIdentityIdQuery : IRequest<byte[]>
{
    public Guid IdentityId { get; set; }
}
