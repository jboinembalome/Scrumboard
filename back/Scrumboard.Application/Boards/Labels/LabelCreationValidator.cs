using FluentValidation;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Application.Boards.Labels;

internal sealed class LabelCreationValidator : AbstractValidator<LabelCreation>
{
    private readonly IBoardsRepository _boardsRepository;

    public LabelCreationValidator(IBoardsRepository boardsRepository)
    {
        _boardsRepository = boardsRepository;
        
        RuleFor(p => p.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} is required.")
            .MaximumLength(255)
                .WithMessage("{PropertyName} must not exceed 255 characters.");
        
        RuleFor(x => x.Colour)
            .Must(ColourExists)
                .WithMessage("{PropertyName} is not supported.");
        
        RuleFor(x => x.BoardId)
            .MustAsync(BoardExistsAsync)
                .WithMessage("{PropertyName} not found.");
    }
    
    private static bool ColourExists(Colour colour) 
        => Colour.SupportedColours.Any(x => Equals(x, colour));
    
    private async Task<bool> BoardExistsAsync(int boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
}
