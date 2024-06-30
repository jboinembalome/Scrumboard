using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

internal sealed class CreateCommentCommandHandler(
    IMapper mapper,
    IActivitiesRepository activitiesRepository,
    ICommentsRepository commentsRepository,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IRequestHandler<CreateCommentCommand, CreateCommentCommandResponse>
{
    public async Task<CreateCommentCommandResponse> Handle(
        CreateCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var createCommentCommandResponse = new CreateCommentCommandResponse();
        
        var comment = mapper.Map<Comment>(request);
        
        await commentsRepository.AddAsync(comment, cancellationToken);
        
        var activity = new Activity(comment.CardId, ActivityType.Added, ActivityField.Comment, string.Empty, request.Message);
        
        await activitiesRepository.AddAsync(activity, cancellationToken);
        
        var user = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        var commentDto = mapper.Map<CommentDto>(comment);
        mapper.Map(user, commentDto.Adherent);
        createCommentCommandResponse.Comment = commentDto;

        return createCommentCommandResponse;
    }
}
