using FluentValidation;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentEditionValidator : AbstractValidator<CommentEdition>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly ICardsRepository _cardsRepository;

    public CommentEditionValidator(
        ICommentsRepository commentsRepository,
        ICardsRepository cardsRepository)
    {
        _commentsRepository = commentsRepository;
        _cardsRepository = cardsRepository;

        RuleFor(p => p.Message)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
        
        RuleFor(x => x.Id)
            .MustAsync(CommentExistsAsync)
            .WithMessage("{PropertyName} not found.");
        
        RuleFor(x => x.CardId)
            .MustAsync(CardExistsAsync)
            .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> CommentExistsAsync(CommentId commentId, CancellationToken cancellationToken)
    {
        var comment = await _commentsRepository.TryGetByIdAsync(commentId, cancellationToken);

        return comment is not null;
    }
    
    private async Task<bool> CardExistsAsync(CardId cardId, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.TryGetByIdAsync(cardId, cancellationToken);

        return card is not null;
    }
}
