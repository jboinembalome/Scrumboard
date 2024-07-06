using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Users.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments.Queries.GetCommentsByCardId;

internal sealed class GetCommentsByCardIdQueryHandler(
    IMapper mapper,
    ICommentsQueryRepository commentsQueryRepository,
    IIdentityService identityService)
    : IRequestHandler<GetCommentsByCardIdQuery, IEnumerable<CommentDto>>
{
    public async Task<IEnumerable<CommentDto>> Handle(
        GetCommentsByCardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var comments = await commentsQueryRepository.GetByCardIdAsync(request.CardId, cancellationToken);

        if (comments.Count == 0)
        {
            return [];
        }
        
        var commentDtos = mapper.Map<IEnumerable<CommentDto>>(comments).ToList();
        
        var users = await identityService.GetListAsync(comments
                .Select(a => a.CreatedBy), cancellationToken);
        
        var userDtos = commentDtos.Select(c => c.User).ToList();

        MapUsers(users, userDtos);

        return commentDtos;
    }

    private void MapUsers(IReadOnlyList<IUser> users, IEnumerable<UserDto> userDtos)
    {
        foreach (var userDto in userDtos)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id);

            if (user is null)
            {
                continue;
            }

            mapper.Map(user, userDto);
        }
    }
}
