using FluentValidation;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardEditionValidator 
    : ListBoardInputBaseValidator<ListBoardEdition>
{
    private readonly IListBoardsRepository _listBoardsRepository;

    public ListBoardEditionValidator(
        IListBoardsRepository listBoardsRepository,
        IBoardsRepository boardsRepository) : base(boardsRepository)
    {
        _listBoardsRepository = listBoardsRepository;
        
        RuleFor(x => x.BoardId)
            .MustAsync(BoardHasListBoardAsync)
                .WithMessage("{PropertyName} does not have the list ({ListBoardId}).");
    }
    
    private async Task<bool> BoardHasListBoardAsync(
        ListBoardEdition listBoardEdition,
        BoardId boardId,
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
