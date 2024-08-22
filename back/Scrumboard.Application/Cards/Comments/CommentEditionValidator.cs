using FluentValidation;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentEditionValidator : AbstractValidator<CommentEdition>
{
    private readonly ICardsRepository _cardsRepository;

    public CommentEditionValidator(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;

        RuleFor(p => p.Message)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
        
        // TODO: Throw NotFoundException instead
        RuleFor(x => x.CardId)
            .MustAsync(CardExistsAsync)
            .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> CardExistsAsync(CardId cardId, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.TryGetByIdAsync(cardId, cancellationToken);

        return card is not null;
    }
}
