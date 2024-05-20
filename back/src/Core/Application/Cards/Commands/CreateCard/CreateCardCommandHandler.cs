using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.ListBoards.Specifications;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

internal sealed class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, CreateCardCommandResponse>
{
    private readonly IAsyncRepository<Card, int> _cardRepository;
    private readonly IAsyncRepository<ListBoard, int> _listBoardRepository;
    private readonly IMapper _mapper;

    public CreateCardCommandHandler(
        IMapper mapper, 
        IAsyncRepository<Card, int> cardRepository, 
        IAsyncRepository<ListBoard, int> listBoardRepository)
    {
        _mapper = mapper;
        _cardRepository = cardRepository;
        _listBoardRepository = listBoardRepository;
    }

    public async Task<CreateCardCommandResponse> Handle(
        CreateCardCommand request, 
        CancellationToken cancellationToken)
    {
        var createCardCommandResponse = new CreateCardCommandResponse();

        var specification = new ListBoardWithCardsSpec(request.ListBoardId);
        var listBoard = await _listBoardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (listBoard is null)
            throw new NotFoundException(nameof(ListBoard), request.ListBoardId);

        var card = _mapper.Map<Card>(request);
        card.ListBoard = listBoard;
        
        card = await _cardRepository.AddAsync(card, cancellationToken);

        createCardCommandResponse.Card = _mapper.Map<CardDetailDto>(card);

        return createCardCommandResponse;
    }
}