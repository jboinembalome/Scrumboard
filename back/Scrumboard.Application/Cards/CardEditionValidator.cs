using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

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
    }
    
    private async Task<bool> CardExistsAsync(int cardId, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.TryGetByIdAsync(cardId, cancellationToken);

        return card is not null;
    }
}
