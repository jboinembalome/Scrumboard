using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Comments.UpdateComment;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Application.Comments.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IAsyncRepository<Comment, int> _commentRepository;
    private readonly IAsyncRepository<Card, int> _cardRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public DeleteCommentCommandHandler(IMapper mapper, IAsyncRepository<Comment, int> commentRepository, IAsyncRepository<Card, int> cardRepository, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _commentRepository = commentRepository;
        _cardRepository = cardRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var specification = new CommentWithAdherentAndCardSpec(request.CommentId);
        var commentToDelete = await _commentRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (commentToDelete == null)
            throw new NotFoundException(nameof(Comment), request.CommentId);

        if (commentToDelete.Adherent.IdentityId != _currentUserService.UserId)
            throw new ForbiddenAccessException();

        var activity = new Activity(ActivityType.Removed, ActivityField.Comment, commentToDelete.Message, string.Empty);
        activity.Adherent = commentToDelete.Adherent;
        commentToDelete.Card.Activities.Add(activity);

        await _commentRepository.DeleteAsync(commentToDelete, cancellationToken);

        return Unit.Value;
    }
}