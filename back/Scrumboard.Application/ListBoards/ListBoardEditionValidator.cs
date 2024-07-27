using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardEditionValidator : AbstractValidator<ListBoardEdition>
{
    private readonly IBoardsRepository _boardsRepository;

    public ListBoardEditionValidator(IBoardsRepository boardsRepository)
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
    
    private async Task<bool> BoardExistsAsync(int boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
}
