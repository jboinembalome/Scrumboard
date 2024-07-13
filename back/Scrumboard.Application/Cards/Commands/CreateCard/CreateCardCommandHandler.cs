﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

internal sealed class CreateCardCommandHandler(
    IMapper mapper,
    ICardsRepository cardsRepository,
    IValidator<CreateCardCommand> createCardCommandValidator)
    : IRequestHandler<CreateCardCommand, CreateCardCommandResponse>
{
    public async Task<CreateCardCommandResponse> Handle(
        CreateCardCommand request, 
        CancellationToken cancellationToken)
    {
        await createCardCommandValidator.ValidateAndThrowAsync(request, cancellationToken);
        
        var createCardCommandResponse = new CreateCardCommandResponse();
        
        var card = mapper.Map<Card>(request);
       
        card = await cardsRepository.AddAsync(card, cancellationToken);

        createCardCommandResponse.Card = mapper.Map<CardDetailDto>(card);

        return createCardCommandResponse;
    }
}
