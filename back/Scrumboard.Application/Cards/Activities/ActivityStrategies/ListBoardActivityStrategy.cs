using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Cards.Activities.ActivityStrategies;

internal sealed class ListBoardActivityStrategy(
    IListBoardsRepository listBoardsRepository) : IChangeActivityStrategy<ListBoardId>
{
    public async Task<IReadOnlyCollection<Activity>> GetChangeActivitiesAsync(
        CardId cardId,
        PropertyValueChange<ListBoardId> listBoardId,
        CancellationToken cancellationToken)
    {
        if (listBoardId.OldValue == listBoardId.NewValue)
        {
            return [];
        }

        var newListBoard = await listBoardsRepository.TryGetByIdAsync(listBoardId.NewValue, cancellationToken)
            .OrThrowResourceNotFoundAsync(listBoardId.NewValue);

        return
        [
            new Activity(
                cardId,
                ActivityType.Updated,
                ActivityField.ListBoard,
                string.Empty,
                newListBoard.Name)
        ];
    }
}
