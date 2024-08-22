using FluentValidation;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Domain.Boards;
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
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Id)
            .MustAsync(BoardExistsAsync)
            .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> BoardExistsAsync(BoardId boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
}
