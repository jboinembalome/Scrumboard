using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.DeleteCard;

public class DeleteCardHandler : IRequestHandler<DeleteCardCommand>
{
    private readonly IAsyncRepository<Card, int> _cardRepository;
    private readonly IMapper _mapper;

    public DeleteCardHandler(IMapper mapper, IAsyncRepository<Card, int> cardRepository)
    {
        _mapper = mapper;
        _cardRepository = cardRepository;
    }

    public async Task<Unit> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var cardToDelete = await _cardRepository.GetByIdAsync(request.CardId, cancellationToken);

        if (cardToDelete == null)
            throw new NotFoundException(nameof(Card), request.CardId);

        await _cardRepository.DeleteAsync(cardToDelete, cancellationToken);

        return Unit.Value;
    }
}