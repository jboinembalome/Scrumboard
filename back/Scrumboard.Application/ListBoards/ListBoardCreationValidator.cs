using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardCreationValidator(IBoardsRepository boardsRepository)
        : ListBoardInputBaseValidator<ListBoardCreation>(boardsRepository)
{
}
