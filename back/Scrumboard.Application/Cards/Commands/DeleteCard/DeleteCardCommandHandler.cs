using MediatR;
using Scrumboard.Application.Cards.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Commands.DeleteCard;

internal sealed class DeleteCardCommandHandler(
    IAsyncRepository<Card, int> cardRepository)
    : IRequestHandler<DeleteCardCommand>
{
    public async Task Handle(
        DeleteCardCommand request, 
        CancellationToken cancellationToken)
    {
        var specification = new CardWithAllSpec(request.CardId);
        var cardToDelete = await cardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (cardToDelete == null)
            throw new NotFoundException(nameof(Card), request.CardId);

        cardToDelete.Activities = [];
        cardToDelete.Checklists = [];
        cardToDelete.Assignees = [];
        cardToDelete.Labels = [];
        cardToDelete.Comments = [];
        
        await cardRepository.DeleteAsync(cardToDelete, cancellationToken);
    }
}
