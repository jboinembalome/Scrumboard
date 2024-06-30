using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments.Commands.DeleteComment;

internal sealed class DeleteCommentCommandHandler(
    IMapper mapper,
    IActivitiesRepository activitiesRepository,
    ICommentsRepository commentsRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteCommentCommand>
{
    private readonly IMapper _mapper = mapper;

    public async Task Handle(
        DeleteCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var commentToDelete = await commentsRepository.TryGetByIdAsync(request.CommentId, cancellationToken);

        if (commentToDelete is null)
            throw new NotFoundException(nameof(Comment), request.CommentId);
        
        // TODO: Add Policy for that?
        if (commentToDelete.CreatedBy != currentUserService.UserId)
            throw new ForbiddenAccessException();
        
        await commentsRepository.DeleteAsync(commentToDelete.Id, cancellationToken);
        
        var activity = new Activity(commentToDelete.CardId, ActivityType.Removed, ActivityField.Comment, commentToDelete.Message, string.Empty);
        
        await activitiesRepository.AddAsync(activity, cancellationToken);
    }
}
