using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Cards.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

internal sealed class CreateCommentCommandHandler(
    IMapper mapper,
    IAsyncRepository<Card, int> cardRepository,
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

        var comment = mapper.Map<Comment>(request);
        
        card.Comments.Add(comment);

        var activity = new Activity(ActivityType.Added, ActivityField.Comment, string.Empty, request.Message);
        card.Activities.Add(activity);
        
        // TODO: Analyze this behavior
        await cardRepository.UpdateAsync(card, cancellationToken);
        
        var user = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        var commentDto = mapper.Map<CommentDto>(comment);

        mapper.Map(user, commentDto.Adherent);
        createCommentCommandResponse.Comment = commentDto;

        return createCommentCommandResponse;
    }
}
