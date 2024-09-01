using FluentValidation;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardEditionValidator : AbstractValidator<ListBoardEdition>
{
    private readonly IListBoardsRepository _listBoardsRepository;
    private readonly IBoardsRepository _boardsRepository;

    public ListBoardEditionValidator(
        IListBoardsRepository listBoardsRepository,
        IBoardsRepository boardsRepository)
    {
        _listBoardsRepository = listBoardsRepository;
        _boardsRepository = boardsRepository;

        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(255)
            .WithMessage("{PropertyName} must not exceed 255 characters.");

        RuleFor(p => p.Position)
            .GreaterThan(0);

        RuleFor(x => x.Id)
            .MustAsync(ListBoardExistsAsync)
            .WithMessage("{PropertyName} not found.");

        RuleFor(x => x.BoardId)
            .MustAsync(BoardExistsAsync)
            .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> ListBoardExistsAsync(ListBoardId listBoardId, CancellationToken cancellationToken)
    {
        var listBoard = await _listBoardsRepository.TryGetByIdAsync(listBoardId, cancellationToken);

        return listBoard is not null;
    }
    
    private async Task<bool> BoardExistsAsync(BoardId boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
}
