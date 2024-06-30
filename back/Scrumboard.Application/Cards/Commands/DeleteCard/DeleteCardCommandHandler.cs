using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Application.Cards.Commands.DeleteCard;

internal sealed class DeleteCardCommandHandler(
    ICardsRepository cardsRepository)
    : IRequestHandler<DeleteCardCommand>
{
    public async Task Handle(
        DeleteCardCommand request, 
        CancellationToken cancellationToken)
    {
        var cardToDelete = await cardsRepository.TryGetByIdAsync(request.CardId, cancellationToken);

        if (cardToDelete is null)
            throw new NotFoundException(nameof(Card), request.CardId);
        
        await cardsRepository.DeleteAsync(cardToDelete.Id, cancellationToken);
    }
}
