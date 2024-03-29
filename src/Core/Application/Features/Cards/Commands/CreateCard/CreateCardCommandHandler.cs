﻿using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Adherents.Specifications;
using Scrumboard.Application.Features.ListBoards.Specifications;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Cards.Commands.CreateCard
{
    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, CreateCardCommandResponse>
    {
        private readonly IAsyncRepository<Card, int> _cardRepository;
        private readonly IAsyncRepository<ListBoard, int> _listBoardRepository;
        private readonly IMapper _mapper;

        public CreateCardCommandHandler(IMapper mapper, IAsyncRepository<Card, int> cardRepository, IAsyncRepository<ListBoard, int> listBoardRepository)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _listBoardRepository = listBoardRepository;
        }

        public async Task<CreateCardCommandResponse> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var createCardCommandResponse = new CreateCardCommandResponse();

            var specification = new ListBoardWithCardsSpec(request.ListBoardId);
            var listBoard = await _listBoardRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (listBoard == null)
                throw new NotFoundException(nameof(ListBoard), request.ListBoardId);

            var card = _mapper.Map<Card>(request);
            card.ListBoard = listBoard;
            card = await _cardRepository.AddAsync(card, cancellationToken);

            createCardCommandResponse.Card = _mapper.Map<CardDetailDto>(card);

            return createCardCommandResponse;
        }
    }
}
