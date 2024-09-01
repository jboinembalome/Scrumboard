using FluentValidation;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardCreationValidator : AbstractValidator<ListBoardCreation>
{
    private readonly IBoardsRepository _boardsRepository;

    public ListBoardCreationValidator(IBoardsRepository boardsRepository)
    {
        _boardsRepository = boardsRepository;

        RuleFor(p => p.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} is required.")
            .MaximumLength(255)
                .WithMessage("{PropertyName} must not exceed 255 characters.");

        RuleFor(p => p.Position)
            .GreaterThan(0);
        
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
