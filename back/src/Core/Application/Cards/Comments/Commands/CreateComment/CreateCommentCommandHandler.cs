using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Specifications;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Cards.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

internal sealed class CreateCommentCommandHandler(
    IMapper mapper,
    IAsyncRepository<Comment, int> commentRepository,
    IAsyncRepository<Card, int> cardRepository,
    IAsyncRepository<Adherent, int> adherentRepository,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IRequestHandler<CreateCommentCommand, CreateCommentCommandResponse>
{
    public async Task<CreateCommentCommandResponse> Handle(
        CreateCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var createCommentCommandResponse = new CreateCommentCommandResponse();

        var cardSpecification = new CardWithActivitiesSpec(request.CardId);
        var card = await cardRepository.FirstOrDefaultAsync(cardSpecification, cancellationToken);

        if (card is null)
            throw new NotFoundException(nameof(Card), request.CardId);

        var specification = new AdherentByUserIdSpec(currentUserService.UserId);
        var adherent = await adherentRepository.FirstAsync(specification, cancellationToken);

        var comment = mapper.Map<Comment>(request);
        comment.Adherent = adherent;
        comment.Card = card;

        var activity = new Activity(ActivityType.Added, ActivityField.Comment, string.Empty, request.Message);
        activity.Adherent = adherent;
        comment.Card.Activities.Add(activity);

        comment = await commentRepository.AddAsync(comment, cancellationToken);

        var user = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        var commentDto = mapper.Map<CommentDto>(comment);

        mapper.Map(user, commentDto.Adherent);
        createCommentCommandResponse.Comment = commentDto;

        return createCommentCommandResponse;
    }
}
