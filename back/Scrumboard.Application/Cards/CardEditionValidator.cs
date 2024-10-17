﻿using FluentValidation;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.Cards;

internal sealed class CardEditionValidator : CardInputBaseValidator<CardEdition>
{
    private readonly ICardsRepository _cardsRepository;

    public CardEditionValidator(
        ICardsRepository cardsRepository,
        IListBoardsRepository listBoardsRepository,
        ILabelsRepository labelsRepository,
        IIdentityService identityService) 
        : base(
            listBoardsRepository, 
            labelsRepository, 
            identityService)
    {
        _cardsRepository = cardsRepository;
        
        RuleFor(x => x.Id)
            .MustAsync(CardExistsAsync)
            .WithMessage("{PropertyName} not found.");
        
        RuleFor(p => p.Position)
            .GreaterThan(0);
    }
    
    private async Task<bool> CardExistsAsync(CardId cardId, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.TryGetByIdAsync(cardId, cancellationToken);

        return card is not null;
    }
}
