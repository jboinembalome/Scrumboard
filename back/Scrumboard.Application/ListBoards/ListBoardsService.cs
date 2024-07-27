using FluentValidation;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardsService(
    IListBoardsQueryRepository listBoardsQueryRepository,
    IListBoardsRepository listBoardsRepository,
    IValidator<ListBoardCreation> listBoardCreationValidator,
    IValidator<ListBoardEdition> listBoardEditionValidator) : IListBoardsService
{
    public async Task<ListBoard> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await listBoardsQueryRepository.TryGetByIdAsync(id, cancellationToken) 
           ?? throw new NotFoundException(nameof(ListBoard), id);

    public async Task<ListBoard> AddAsync(ListBoardCreation listBoardCreation, CancellationToken cancellationToken = default)
    {
        await listBoardCreationValidator.ValidateAndThrowAsync(listBoardCreation, cancellationToken);
        
        var listBoard = await listBoardsRepository.AddAsync(listBoardCreation, cancellationToken);
        
        return listBoard;
    }

    public async Task<ListBoard> UpdateAsync(ListBoardEdition listBoardEdition, CancellationToken cancellationToken = default)
    {
        _ = await listBoardsRepository.TryGetByIdAsync(listBoardEdition.Id, cancellationToken) 
                           ?? throw new NotFoundException(nameof(ListBoard), listBoardEdition.Id);
        
        await listBoardEditionValidator.ValidateAndThrowAsync(listBoardEdition, cancellationToken);
        
        return await listBoardsRepository.UpdateAsync(listBoardEdition, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await listBoardsRepository.TryGetByIdAsync(id, cancellationToken) 
            ?? throw new NotFoundException(nameof(ListBoard), id);
        
        await listBoardsRepository.DeleteAsync(id, cancellationToken);
    }
}
