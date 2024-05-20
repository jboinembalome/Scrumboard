using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.ListBoards.Specifications;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

internal sealed class CreateCardCommandHandler(
    IMapper mapper,
    IAsyncRepository<Card, int> cardRepository,
    IAsyncRepository<ListBoard, int> listBoardRepository)
    : IRequestHandler<CreateCardCommand, CreateCardCommandResponse>
{
    public async Task<CreateCardCommandResponse> Handle(
        CreateCardCommand request, 
        CancellationToken cancellationToken)
    {
        var createCardCommandResponse = new CreateCardCommandResponse();

        var specification = new ListBoardWithCardsSpec(request.ListBoardId);
        var listBoard = await listBoardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (listBoard is null)
            throw new NotFoundException(nameof(ListBoard), request.ListBoardId);

        var card = mapper.Map<Card>(request);
        card.ListBoard = listBoard;
        
        card = await cardRepository.AddAsync(card, cancellationToken);

        createCardCommandResponse.Card = mapper.Map<CardDetailDto>(card);

        return createCardCommandResponse;
    }
}
