using AutoMapper;
using FluentValidation;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Cards;

internal sealed class CardsService(
    IMapper mapper,
    ICardsRepository cardsRepository,
    ICardsQueryRepository cardsQueryRepository,
    IValidator<CardCreation> cardCreationValidator,
    IValidator<CardEdition> cardEditionValidator) : ICardsService
{
    public async Task<bool> ExistsAsync(
        CardId id, 
        CancellationToken cancellationToken = default)
    {
        var card = await cardsRepository.TryGetByIdAsync(id, cancellationToken);

        return card is not null;
    }

    public Task<IReadOnlyList<Card>> GetByListBoardIdAsync(
        ListBoardId listBoardId, 
        CancellationToken cancellationToken = default)
        => cardsQueryRepository.GetByListBoardIdAsync(listBoardId, cancellationToken);

    public Task<Card> GetByIdAsync(
        CardId id, 
        CancellationToken cancellationToken = default)
        => cardsQueryRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);

    public async Task<Card> AddAsync(
        CardCreation cardCreation, 
        CancellationToken cancellationToken = default)
    {
        await cardCreationValidator.ValidateAndThrowAsync(cardCreation, cancellationToken);

        var card = mapper.Map<Card>(cardCreation);
        
        await cardsRepository.AddAsync(card, cancellationToken);
        
        return card;
    }

    public async Task<Card> UpdateAsync(
        CardEdition cardEdition, 
        CancellationToken cancellationToken = default)
    {
        await cardEditionValidator.ValidateAndThrowAsync(cardEdition, cancellationToken);
        
        var card = await cardsRepository.TryGetByIdAsync(cardEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(cardEdition.Id);

        card.Update(
           name: cardEdition.Name,
           description: cardEdition.Description,
           dueDate: cardEdition.DueDate,
           position: cardEdition.Position,
           listBoardId: cardEdition.ListBoardId,
           assigneeIds: cardEdition.AssigneeIds,
           labelIds: cardEdition.LabelIds);

        cardsRepository.Update(card);

        return card;
    }

    public async Task DeleteAsync(
        CardId id, 
        CancellationToken cancellationToken = default)
    { 
        await cardsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await cardsRepository.DeleteAsync(id, cancellationToken);
    }
}
