using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

internal sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    private readonly ICardsRepository _cardsRepository;

    public CreateCommentCommandValidator(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
        
        RuleFor(p => p.Message)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(1000).WithMessage("{PropertyName} must not exceed 50 characters.");
        
        RuleFor(x => x.CardId)
            .NotEmpty()
            .MustAsync(CardExists)
            .WithMessage("Card ({PropertyValue}) not found.");
    }
    
    private async Task<bool> CardExists(int cardId, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.TryGetByIdAsync(cardId, cancellationToken);
        
        return card is not null;
    }
}
