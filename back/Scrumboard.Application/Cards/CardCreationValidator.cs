using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.Cards;

internal sealed class CardCreationValidator(
    IListBoardsRepository listBoardsRepository,
    ILabelsRepository labelsRepository,
    IIdentityService identityService)
    : CardInputBaseValidator<CardCreation>(
        listBoardsRepository,
        labelsRepository,
        identityService);
