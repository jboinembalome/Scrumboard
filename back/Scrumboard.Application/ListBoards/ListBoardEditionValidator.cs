using FluentValidation;
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

        RuleFor(x => x.Id)
            .MustAsync(ListBoardExistsAsync)
            .WithMessage("{PropertyName} not found.");
        
        RuleFor(x => x.BoardId)
            .MustAsync(BoardExistsAsync)
                .WithMessage("{PropertyName} not found.")
            .MustAsync(BoardHasListBoardAsync)
                .WithMessage("{PropertyName} does not have the list ({ListBoardId}).");
    }
    
    private async Task<bool> ListBoardExistsAsync(int id, CancellationToken cancellationToken)
    {
        var listBoard = await _listBoardsRepository.TryGetByIdAsync(id, cancellationToken);

        return listBoard is not null;
    }
    
    private async Task<bool> BoardExistsAsync(int boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
    
    private async Task<bool> BoardHasListBoardAsync(
        ListBoardEdition listBoardEdition,
        int boardId,
        ValidationContext<ListBoardEdition> validationContext,
        CancellationToken cancellationToken)
    {
        var listBoard = await _listBoardsRepository.TryGetByIdAsync(listBoardEdition.Id, cancellationToken);

        if (listBoard?.BoardId == boardId)
        {
            return true;
        }
        
        validationContext.MessageFormatter
            .AppendArgument("ListBoardId", listBoardEdition.Id);
        
        return false;
    }
}
