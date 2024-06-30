using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Cards.Dtos;
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
        
        var adherentDtos = commentDtos.Select(c => c.Adherent).ToList();

        MapUsers(users, adherentDtos);

        return commentDtos;
    }

    private void MapUsers(IReadOnlyList<IUser> users, IEnumerable<AdherentDto> adherents)
    {
        foreach (var adherent in adherents)
        {
            var user = users.FirstOrDefault(u => u.Id == adherent.Id);

            if (user is null)
            {
                continue;
            }

            mapper.Map(user, adherent);
        }
    }
}
