using MediatR;

namespace Scrumboard.Application.Adherents.Queries.GetAvatarByIdentityId;

public sealed class GetAvatarByIdentityIdQuery : IRequest<byte[]>
{
    public string IdentityId { get; set; } = string.Empty;
}
