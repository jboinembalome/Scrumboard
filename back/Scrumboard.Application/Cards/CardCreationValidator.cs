using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Application.Cards;

internal sealed class CardCreationValidator(
    IListBoardsRepository listBoardsRepository,
    ILabelsRepository labelsRepository,
    IIdentityService identityService)
    : CardInputBaseValidator<CardCreation>(
        listBoardsRepository,
        labelsRepository,
        identityService);
