using AutoMapper;
using MediatR;
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
        var cardToDelete = await cardRepository.GetByIdAsync(request.CardId, cancellationToken);

        if (cardToDelete == null)
            throw new NotFoundException(nameof(Card), request.CardId);

        await cardRepository.DeleteAsync(cardToDelete, cancellationToken);
    }
}
