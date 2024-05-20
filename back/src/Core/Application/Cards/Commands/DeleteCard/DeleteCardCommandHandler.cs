using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Commands.DeleteCard;

internal sealed class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand>
{
    private readonly IAsyncRepository<Card, int> _cardRepository;
    private readonly IMapper _mapper;

    public DeleteCardCommandHandler(
        IMapper mapper, 
        IAsyncRepository<Card, int> cardRepository)
    {
        _mapper = mapper;
        _cardRepository = cardRepository;
    }

    public async Task Handle(
        DeleteCardCommand request, 
        CancellationToken cancellationToken)
    {
        var cardToDelete = await _cardRepository.GetByIdAsync(request.CardId, cancellationToken);

        if (cardToDelete == null)
            throw new NotFoundException(nameof(Card), request.CardId);

        await _cardRepository.DeleteAsync(cardToDelete, cancellationToken);
    }
}
