using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Comments.Specifications;
using Scrumboard.Application.Cards.Specifications;
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
    private readonly IMapper _mapper = mapper;

    public async Task Handle(
        DeleteCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var specification = new CommentWithAdherentSpec(request.CommentId);
        var commentToDelete = await commentRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (commentToDelete is null)
            throw new NotFoundException(nameof(Comment), request.CommentId);

        
        if (commentToDelete.CreatedBy != currentUserService.UserId)
            throw new ForbiddenAccessException();

        var cardSpecification = new CardByCommentSpec(commentToDelete.Id);
        var card = await cardRepository.FirstOrDefaultAsync(cardSpecification, cancellationToken);
        
        if (card is null)
            throw new NotFoundException(nameof(Card));
        
        card.Comments.Remove(commentToDelete);
        
        var activity = new Activity(ActivityType.Removed, ActivityField.Comment, commentToDelete.Message, string.Empty);
        card.Activities.Add(activity);
        
        // TODO: Analyze this behavior
        await cardRepository.UpdateAsync(card, cancellationToken);

        await commentRepository.DeleteAsync(commentToDelete, cancellationToken);
    }
}
