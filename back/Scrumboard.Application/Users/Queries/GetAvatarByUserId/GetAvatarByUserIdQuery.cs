using MediatR;

namespace Scrumboard.Application.Users.Queries.GetAvatarByUserId;

public sealed class GetAvatarByUserIdQuery : IRequest<byte[]>
{
    public string UserId { get; set; } = string.Empty;
}
