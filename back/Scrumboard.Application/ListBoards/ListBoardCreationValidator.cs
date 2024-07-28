using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardCreationValidator(IBoardsRepository boardsRepository)
        : ListBoardInputBaseValidator<ListBoardCreation>(boardsRepository)
{
}
