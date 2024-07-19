using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentCreationValidator : AbstractValidator<CommentCreation>
{
    private readonly ICardsRepository _cardsRepository;

    public CommentCreationValidator(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
        
        RuleFor(p => p.Message)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
        
        RuleFor(x => x.CardId)
            .MustAsync(CardExistsAsync)
            .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> CardExistsAsync(int cardId, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.TryGetByIdAsync(cardId, cancellationToken);

        return card is not null;
    }
}
