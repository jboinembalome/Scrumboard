using FluentValidation;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

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
        
        RuleFor(x => x.BoardId)
            .MustAsync(BoardExistsAsync)
                .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> BoardExistsAsync(BoardId boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
}
