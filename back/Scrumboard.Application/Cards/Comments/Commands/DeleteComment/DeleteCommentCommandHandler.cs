using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Comments.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Comments.Commands.DeleteComment;

internal sealed class DeleteCommentCommandHandler(
    IMapper mapper,
    IAsyncRepository<Comment, int> commentRepository,
    IAsyncRepository<Card, int> cardRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteCommentCommand>
{
    private readonly IAsyncRepository<Card, int> _cardRepository = cardRepository;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(
        DeleteCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var specification = new CommentWithAdherentAndCardSpec(request.CommentId);
        var commentToDelete = await commentRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (commentToDelete == null)
            throw new NotFoundException(nameof(Comment), request.CommentId);

        if (commentToDelete.Adherent.IdentityId != currentUserService.UserId)
            throw new ForbiddenAccessException();

        var activity = new Activity(ActivityType.Removed, ActivityField.Comment, commentToDelete.Message, string.Empty);
        activity.Adherent = commentToDelete.Adherent;
        commentToDelete.Card.Activities.Add(activity);

        await commentRepository.DeleteAsync(commentToDelete, cancellationToken);
    }
}
