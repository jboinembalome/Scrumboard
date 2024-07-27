using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards;

internal sealed class BoardEditionValidator : AbstractValidator<BoardEdition>
{
    private readonly IBoardsRepository _boardsRepository;

    public BoardEditionValidator(
        IBoardsRepository boardsRepository)
    {
        _boardsRepository = boardsRepository;
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        
        RuleFor(x => x.Id)
            .MustAsync(BoardExistsAsync)
            .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> BoardExistsAsync(int boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
}
